namespace Server.Services.Dtos
{
    public class StudentInfoDto
    {
        public string FullName { get; set; } = null!;

        public GroupDto Group { get; set; }

        public FacultyDto Faculty { get; set; }

        public bool Headman { get; set; }
    }
}
