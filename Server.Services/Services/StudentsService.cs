using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Interfaces.ExternalInterfaces;
using Server.Models.Models;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.StudentDtos;
using Server.Services.Mappings;
using Server.Services.Parsers;
using Server.Services.Services.StaticServices;

namespace Server.Services.Services;

public class StudentsService(IStudentRepository studentRepository, IStudentDtoRepository studentDtoRepository,
    IGroupRepository groupRepository, IEmailService emailService)
{
    public async Task<IEnumerable<StudentWithRecordsDto>> GetWithLastReocorsByGroupId(uint groupId)
    {
        var groupInfo = await groupRepository.GetById(groupId) ??
            throw new NotFoundException("Групу не знайдено");

        var holding = CalcuationService.CalculateLastHoldingForGroup(groupInfo);

        var students = await studentRepository.GetWithLastReocorsByGroupId(groupId, (short)holding);
        return students.Select(StudentMapper.MapToStudentWithRecordsDto);
    }

    public async Task<IReadOnlyList<StudentWithAllRecordsInfo>> GetWithAllRecordsByGroupId(uint groupId) =>
        await studentDtoRepository.GetWithAllRecordsByGroupId(groupId);

    public async Task<StudentRegistryDto> AddStudent(StudentRegistryDto studentRegistry)
    {
        var password = GeneratorService.GeneratePassword();
        var user = CreateAddModel(studentRegistry, password);

        var newStudent = await studentRepository.Add(user);

        await emailService.SendEmailAsync(user.Email, "Реєстрація у системі проведення вибору дисциплін",
            GeneratorService.GenerateRegistrationEmailBody(user.Email, password));

        return UserMapper.MapToStudentRegistry(newStudent);
    }

    public async Task<IEnumerable<StudentRegistryDto>> AddStudents(List<StudentRegistryDto> studentsRegistry)
    {
        var passwordList = new Dictionary<string, string>(studentsRegistry.Count);

        var users = studentsRegistry.Select(studentRegistry =>
        {
            var password = GeneratorService.GeneratePassword();
            passwordList[studentRegistry.Email] = password;
            return CreateAddModel(studentRegistry, password);
        }).ToList();

        var newUsers = await studentRepository.AddRange(users, users.Select(u => u.Student));

        var messages = newUsers.Select(user => (user.Email,
        GeneratorService.GenerateRegistrationEmailBody(user.Email, passwordList[user.Email])));

        await emailService.SendEmailsAsync("Реєстрація у системі проведення вибору дисциплін", messages);

        return newUsers.Select(UserMapper.MapToStudentRegistry);
    }

    public async Task<StudentRegistryDto> UpdateOrThrow(StudentRegistryDto studentRegistry)
    {
        if (studentRegistry.StudentId is null) throw new BadRequestException("Невалідні вхідні дані");

        var byteStudentId = UlidIdParser.ParseId(studentRegistry.StudentId);
        var student = StudentMapper.MapToStudentWithUserWithoutId(studentRegistry);

        student.StudentId = byteStudentId;
        student.User.UserId = byteStudentId;

        var updatedStudent = await studentRepository.Update(student) ??
            throw new NotFoundException("Вказаного студента не знайдено");

        return StudentMapper.MapToStudentRegistryDto(updatedStudent);
    }

    public async Task DeleteOrThrow(string studentId, string? requestRole)
    {
        var byteStudentId = UlidIdParser.ParseId(studentId);

        var result = await studentRepository.Delete(byteStudentId, RoleParser.Parse(requestRole), (short)(DateTime.Today.Year - 1));

        result.ThrowIfFailed("Вказаного студента не знайдено або недостатній рівень доступу",
            "Неможливо видалити, оскільки до студента є прив'язані дані за останній рік");
    }

    private static User CreateAddModel(StudentRegistryDto student, string password)
    {
        var user = UserMapper.MapToUserFromStudentWithoutId(student);

        var id = GeneratorService.GenerateByteUlid();
        user.UserId = id;
        user.Student.StudentId = id;

        var salt = GeneratorService.GenerateSalt();
        var passwordHash = HasherService.GetPBKDF2Hash(password, salt);

        user.Salt = salt;
        user.Password = passwordHash;

        return user;
    }
}
