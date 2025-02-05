namespace Server.Services.Dtos
{
    public class WorkerInfoDto
    {
        public string FullName { get; set; }

        public FacultyDto Faculty { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public uint? Group { get; set; }
    }
}
