using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class WorkersService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IEmailService _emailService;

        public WorkersService(IWorkerRepository workerRepository, IEmailService emailService)
        {
            _workerRepository = workerRepository;
            _emailService = emailService;
        }

        public async Task<int> GetCount(uint? facultyFilter)
        {
            var workersCount = facultyFilter is null ? await _workerRepository.GetCount() :
                await _workerRepository.GetCount(facultyFilter.Value);

            return workersCount;
        }

        public async Task<IEnumerable<WorkerFullInfoDto>> GetWorkers(int pageNumber, int pageSize, uint? facultyFilter)
        {
            var workers = facultyFilter is null ? await _workerRepository.GetWorkers(pageNumber, pageSize) :
                await _workerRepository.GetWorkers(pageNumber, pageSize, facultyFilter.Value);

            return workers.Select(UserMapper.MapToWorkerFullInfoDto);
        }

        public async Task<WorkerFullInfoDto> AddWorker(WorkerFullInfoDto worker)
        {
            var user = UserMapper.MapToUserWithoutId(worker);

            Ulid ulid = Ulid.NewUlid();

            user.UserId = ulid.ToByteArray();
            user.Worker.WorkerId = ulid.ToByteArray();

            var salt = GeneratorService.GenerateSalt();

            //For development purposes

            var password = "Test1234";
            //var password = GeneratorService.GeneratePassword();

            var passwordHash = HasherService.GetPBKDF2Hash(password, salt);

            user.Salt = salt;
            user.Password = passwordHash;

            var newWorker = await _workerRepository.Add(user);

            //While developmnet

            //await _emailService.SendEmailAsync(user.Email, "Реєстрація у системі проведення вибору дисциплін",
            //    "Доброго дня!\n" +
            //    "Не забудьте після входу змінити тимчасовий пароль,\n" +
            //    "і надійно його зберігайте.\n" +
            //    $"Ваш логін для входу: {user.Email}\n" +
            //    $"Тимчасовий пароль: {password}\n" +
            //    "Вхід через:\n" +
            //    "\n\nЗ повагою, ДНУ.");

            return UserMapper.MapToWorkerFullInfoDto(newWorker);
        }

        public async Task<WorkerFullInfoDto?> UpdateWorker(WorkerFullInfoDto worker)
        {
            var user = UserMapper.MapToUserWithoutId(worker);

            var isSuccess = Ulid.TryParse(worker.Id, out Ulid ulidWorkerId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteWorkerId = ulidWorkerId.ToByteArray();

            user.UserId = byteWorkerId;
            user.Worker.WorkerId = byteWorkerId;

            var updatedWorker = await _workerRepository.Update(user);

            if (updatedWorker is null)
                return null;

            return UserMapper.MapToWorkerFullInfoDto(updatedWorker);
        }

        public async Task<bool?> DeleteWorker(string workerId)
        {
            var isSuccess = Ulid.TryParse(workerId, out Ulid ulidWorkerId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            return await _workerRepository.Delete(ulidWorkerId.ToByteArray());
        }
    }
}
