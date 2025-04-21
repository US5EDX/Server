using Server.Models.Interfaces;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System.Xml.XPath;

namespace Server.Services.Services
{
    public class StudentsService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentDtoRepository _studentDtoRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IEmailService _emailService;

        public StudentsService(IStudentRepository studentRepository, IStudentDtoRepository studentDtoRepository,
            IGroupRepository groupRepository, IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _studentDtoRepository = studentDtoRepository;
            _groupRepository = groupRepository;
            _emailService = emailService;
        }

        public async Task<IEnumerable<StudentWithRecordsDto>> GetWithLastReocorsByGroupId(uint groupId)
        {
            var groupInfo = await _groupRepository.GetById(groupId);

            if (groupInfo == null)
                throw new Exception("Групу не знайдено");

            var holding = CalcuationService.CalculateLastHoldingForGroup(groupInfo);

            var students = await _studentRepository.GetWithLastReocorsByGroupId(groupId, (short)holding);

            return students.Select(StudentMapper.MapToStudentWithRecordsDto);
        }

        public async Task<object> GetWithAllRecordsByGroupId(uint groupId)
        {
            var students = await _studentDtoRepository.GetWithAllRecordsByGroupId(groupId);
            return students;
        }

        public async Task<StudentRegistryDto> AddStudent(StudentRegistryDto studentRegistry)
        {
            var user = UserMapper.MapToUserFromStudentWithoutId(studentRegistry);

            Ulid ulid = Ulid.NewUlid();

            user.UserId = ulid.ToByteArray();
            user.Student.StudentId = ulid.ToByteArray();

            var salt = GeneratorService.GenerateSalt();

            //For development purposes

            var password = "Test1234";
            //var password = GeneratorService.GeneratePassword();

            var passwordHash = HasherService.GetPBKDF2Hash(password, salt);

            user.Salt = salt;
            user.Password = passwordHash;

            var newStudent = await _studentRepository.Add(user);

            //While developmnet

            //await _emailService.SendEmailAsync(user.Email, "Реєстрація у системі проведення вибору дисциплін",
            //    "Доброго дня!\n" +
            //    "Не забудьте після входу змінити тимчасовий пароль,\n" +
            //    "і надійно його зберігайте.\n" +
            //    $"Ваш логін для входу: {user.Email}\n" +
            //    $"Тимчасовий пароль: {password}\n" +
            //    "Вхід через:\n" +
            //    "\n\nЗ повагою, ДНУ.");

            return UserMapper.MapToStudentRegistry(newStudent);
        }

        public async Task<IEnumerable<StudentRegistryDto>> AddStudents(IEnumerable<StudentRegistryDto> studentsRegistry)
        {
            var passwordList = new List<string>(studentsRegistry.Count());

            var users = studentsRegistry.Select(studentRegistry =>
            {
                var user = UserMapper.MapToUserFromStudentWithoutId(studentRegistry);

                Ulid ulid = Ulid.NewUlid();

                user.UserId = ulid.ToByteArray();
                user.Student.StudentId = ulid.ToByteArray();

                var salt = GeneratorService.GenerateSalt();

                //For development purposes

                var password = "Test1234";
                //var password = GeneratorService.GeneratePassword();

                passwordList.Add(password);

                var passwordHash = HasherService.GetPBKDF2Hash(password, salt);

                user.Salt = salt;
                user.Password = passwordHash;

                return user;
            }).ToList();

            var newUsers = await _studentRepository.AddRange(users);

            //While developmnet

            //int index = 0;

            //foreach (var newUser in newUsers)
            //{
            //    await _emailService.SendEmailAsync(newUser.Email, "Реєстрація у системі проведення вибору дисциплін",
            //        "Доброго дня!\n" +
            //        "Не забудьте після входу змінити тимчасовий пароль,\n" +
            //        "і надійно його зберігайте.\n" +
            //        $"Ваш логін для входу: {newUser.Email}\n" +
            //        $"Тимчасовий пароль: {passwordList[index]}\n" +
            //        "Вхід через:\n" +
            //        "\n\nЗ повагою, ДНУ.");

            //    index++;
            //}

            return newUsers.Select(UserMapper.MapToStudentRegistry);
        }

        public async Task<StudentRegistryDto?> UpdateStudent(StudentRegistryDto studentRegistry)
        {
            var student = StudentMapper.MapToStudentWithUserWithoutId(studentRegistry);

            var isSuccess = Ulid.TryParse(studentRegistry.StudentId, out Ulid ulidStudentId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            var byteStudentId = ulidStudentId.ToByteArray();

            student.StudentId = byteStudentId;
            student.User.UserId = byteStudentId;

            var updatedStudent = await _studentRepository.Update(student);

            if (updatedStudent is null)
                return null;

            return StudentMapper.MapToStudentRegistryDto(updatedStudent);
        }

        public async Task<bool?> DeleteStudent(string studentId, int requestUserId)
        {
            var isSuccess = Ulid.TryParse(studentId, out Ulid ulidStudentId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            return await _studentRepository.Delete(ulidStudentId.ToByteArray(), requestUserId);
        }
    }
}
