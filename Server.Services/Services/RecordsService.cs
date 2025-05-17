using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Converters;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.RecordDtos;
using Server.Services.Dtos.StudentDtos;
using Server.Services.Mappings;
using Server.Services.Parsers;
using Server.Services.Services.StaticServices;

namespace Server.Services.Services;

public class RecordsService(IRecordRepository recordRepository, IRecordDtoRepository recordDtoRepository,
    IHoldingRepository holdingRepository, IGroupRepository groupRepository)
{
    public async Task<IReadOnlyList<RecordWithStudentInfoDto>> GetSignedStudents(uint disciplineId, Semesters semester) =>
        await recordDtoRepository.GetRecordsWithStudentInfo(disciplineId, semester);

    public async Task<IReadOnlyList<RecordWithDisciplineInfoDto>> GetRecordsByStudentIdAndYear(string studentId, short year)
    {
        var byteStudentId = UlidIdParser.ParseId(studentId);
        return await recordDtoRepository.GetByStudentIdAndYear(byteStudentId, year);
    }

    public async Task<IReadOnlyList<RecordShortDisciplineInfoDto>> GetWithDisciplineShort(string? studentId, short year)
    {
        if (studentId is null) throw new BadRequestException("Невалідний Id");

        var byteStudentId = UlidIdParser.ParseId(studentId);
        return await recordDtoRepository.GetWithDisciplineShort(byteStudentId, year);
    }

    public async Task<IReadOnlyList<StudentYearsRecordsDto>> GetMadeChoices(string? studentId)
    {
        if (studentId is null) throw new BadRequestException("Невалідний Id");

        var byteStudentId = UlidIdParser.ParseId(studentId);

        return await recordDtoRepository.GetMadeChoices(byteStudentId);
    }

    public async Task<IEnumerable<StudentYearRecordsDto>> GetByStudentAndGroupIdOrThrow(string studentId, uint groupId)
    {
        var byteStudentId = UlidIdParser.ParseId(studentId);

        var groupInfo = await groupRepository.GetById(groupId) ?? throw new NotFoundException("Групу не знайдено");

        var firstVisibleYear = groupInfo.HasEnterChoise ? groupInfo.AdmissionYear : groupInfo.AdmissionYear + 1;
        var lastVisibleYear = CalcuationService.CalculateLastHoldingForGroup(groupInfo);

        if (lastVisibleYear < firstVisibleYear) return [];

        var yearsRange = Enumerable.Range(firstVisibleYear, lastVisibleYear - firstVisibleYear + 1).ToHashSet();
        var years = await holdingRepository.GetRequestedYears(yearsRange);

        var records = await recordDtoRepository.GetStudentRecordsByYears(byteStudentId, years);

        var groupedRecords = records.GroupBy(r => r.Holding).ToDictionary(pair => pair.Key, pair => pair.ToList());

        foreach (var year in years) groupedRecords.TryAdd(year, []);

        return groupedRecords.OrderByDescending(gr => gr.Key).Select(pair =>
        {
            var semesterGroups = pair.Value.GroupBy(r => r.Semester)
            .ToDictionary(g => g.Key, g => g.Select(RecordMapper.MapToPairDto).ToList());

            return new StudentYearRecordsDto
            {
                EduYear = pair.Key,
                Nonparsemester = semesterGroups.GetValueOrDefault(Semesters.Fall, []),
                Parsemester = semesterGroups.GetValueOrDefault(Semesters.Spring, [])
            };
        });
    }

    public async Task<RecordWithDisciplineInfoDto> AddRecord(RecordRegistryDto record)
    {
        record.RecordId = 0;

        var newRecordId = await recordRepository.Add(RecordMapper.MapToRecord(record));

        return await recordDtoRepository.GetWithDisciplineById(newRecordId) ??
            throw new NotFoundException("Не вдалось знайти додану дисципліну");
    }

    public async Task<RecordWithDisciplineInfoDto> UpdateOrThrow(RecordRegistryDto record)
    {
        if (record.RecordId is null) throw new BadRequestException("Невалідні дані");

        var updatedRecordId = await recordRepository.Update(RecordMapper.MapToRecord(record)) ??
            throw new NotFoundException("Вказаний запис не знайдена");

        return await recordDtoRepository.GetWithDisciplineById(updatedRecordId) ??
            throw new NotFoundException("Не вдалось знайти оновлену дисципліну");
    }

    public async Task DeleteOrThrow(uint recordId)
    {
        var result = await recordRepository.Delete(recordId);

        result.ThrowIfFailed("Вказаний запис не знайдено", "Неможливо видалити, оскільки запис не є відхиленим");
    }

    public async Task UpdateStatusOrThrow(uint recordId, RecordStatus status)
    {
        var isSuccess = await recordRepository.UpdateStatus(recordId, status);

        if (!isSuccess) throw new NotFoundException("Запис не знайдено");
    }

    public async Task<uint> RegisterRecordOrThrow(RecordRegistryWithoutStudent inRecord, string? studentId)
    {
        if (studentId is null) throw new BadRequestException("Невалідний Id");

        var byteStudentId = UlidIdParser.ParseId(studentId);

        var kiyvDate = KiyvDateTimeConverter.ConvertUtcToKyivDate(DateTime.UtcNow);

        var holding = await holdingRepository.GetLast();

        if (holding is null || holding.StartDate > kiyvDate || holding.EndDate < kiyvDate || inRecord.Holding != holding.EduYear)
            throw new BadRequestException("Вибір недоступний");

        var groupInfo = await groupRepository.GetGroupInfoByStudentId(byteStudentId) ??
            throw new BadRequestException("Проблеми з ідентифікатором користувача");

        var course = CalcuationService.CalculateGroupCourse(groupInfo);

        if (course == 0 || course == groupInfo.DurationOfStudy ||
            (holding.EduYear == groupInfo.AdmissionYear && !groupInfo.HasEnterChoise))
            throw new BadRequestException("Вибір для вас не запланований");

        course += groupInfo.ChoiceDifference;

        byte courseMask = CourseToCourseMaskConverter.ConvertToAdjustedCourseMask(course, groupInfo.HasEnterChoise);

        var record = RecordMapper.MapToRecord(inRecord);

        record.StudentId = byteStudentId;

        return record.RecordId == 0 ?
            await recordRepository.AddRecordOrThrow(record, groupInfo.EduLevel, courseMask, inRecord.Semester == Semesters.Fall ?
            groupInfo.Nonparsemester : groupInfo.Parsemester) :
            await recordRepository.UpdateRecordOrThrow(record, groupInfo.EduLevel, courseMask);
    }
}
