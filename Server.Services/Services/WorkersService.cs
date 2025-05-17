using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Interfaces.ExternalInterfaces;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.UserDtos;
using Server.Services.Dtos.WorkerDtos;
using Server.Services.Mappings;
using Server.Services.Parsers;
using Server.Services.Services.StaticServices;

namespace Server.Services.Services;

public class WorkersService(IWorkerRepository workerRepository, IWorkerDtoRepository workerDtoRepository,
    IEmailService emailService)
{
    public async Task<int> GetCount(uint? facultyFilter) => facultyFilter is null ?
        await workerRepository.GetCount() : await workerRepository.GetCount(facultyFilter.Value);

    public async Task<IEnumerable<UserFullInfoDto>> GetWorkers(int pageNumber, int pageSize, uint? facultyFilter)
    {
        var workers = facultyFilter is null ? await workerRepository.GetWorkers(pageNumber, pageSize) :
            await workerRepository.GetWorkers(pageNumber, pageSize, facultyFilter.Value);

        return workers.Select(UserMapper.MapToUserFullInfoDto);
    }

    public async Task<IReadOnlyList<WorkerShortInfoDto>> GetByFacultyAndFullName(uint faculty, string fullName) =>
        await workerDtoRepository.GetByFacultyAndFullName(faculty, fullName);

    public async Task<UserFullInfoDto> AddWorker(WorkerRegistryDto worker, string? requestUserRole)
    {
        if (worker.Role <= RoleParser.Parse(requestUserRole)) throw new ForbidException("Неможливо виконати дію");

        var user = UserMapper.MapToUserFromWorkerWithoutId(worker);

        var id = GeneratorService.GenerateByteUlid();

        user.UserId = id;
        user.Worker.WorkerId = id;

        var salt = GeneratorService.GenerateSalt();
        var password = GeneratorService.GeneratePassword();
        var passwordHash = HasherService.GetPBKDF2Hash(password, salt);

        user.Salt = salt;
        user.Password = passwordHash;

        var newWorker = await workerRepository.Add(user);

        var body = GeneratorService.GenerateRegistrationEmailBody(user.Email, password);
        await emailService.SendEmailAsync(user.Email, "Реєстрація у системі проведення вибору дисциплін", body);

        return UserMapper.MapToUserFullInfoDto(newWorker);
    }

    public async Task<UserFullInfoDto> UpdateOrThrow(WorkerRegistryDto worker, string? requestUserRole)
    {
        if (worker.WorkerId is null) throw new BadRequestException("Невалідні вхідні дані");

        if (worker.Role <= RoleParser.Parse(requestUserRole)) throw new ForbidException("Неможливо виконати дію");

        var user = UserMapper.MapToUserFromWorkerWithoutId(worker);

        var byteWorkerId = UlidIdParser.ParseId(worker.WorkerId);

        user.UserId = byteWorkerId;
        user.Worker.WorkerId = byteWorkerId;

        var updatedWorker = await workerRepository.Update(user) ??
            throw new NotFoundException("Вказаного користувача не знайдено");

        return UserMapper.MapToUserFullInfoDto(updatedWorker);
    }

    public async Task DeleteOrThrow(string workerId, string? requestUserRole)
    {
        var byteWorkerId = UlidIdParser.ParseId(workerId);

        var result = await workerRepository.Delete(byteWorkerId, RoleParser.Parse(requestUserRole),
            (short)(DateTime.Today.Year - 1));

        result.ThrowIfFailed("Вказаного співобітника не знайдено або недостатній рівень доступу",
            "Неможливо видалити, оскільки до співробітника є прив'язані дані");
    }
}
