namespace Server.Services.Dtos
{
    public class StudentInfoDto
    {
        public StudentInfoDto(byte role) { Role = role; }

        public byte[] StudentId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public GroupDto Group { get; set; }

        public FacultyDto Faculty { get; set; }

        public bool Headman { get; set; }

        public byte Role { get; set; }
    }
}
