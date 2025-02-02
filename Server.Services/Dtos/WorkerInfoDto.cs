namespace Server.Services.Dtos
{
    public class WorkerInfoDto
    {
        public WorkerInfoDto(byte role) { Role = role; }

        public byte[] WorkerId { get; set; }

        public string FullName { get; set; }

        public FacultyDto Faculty { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public uint? Group { get; set; }

        public byte Role { get; set; }
    }
}
