namespace Server.Services.Dtos
{
    public class DisciplineInfoForStudent
    {
        public uint DisciplineId { get; set; }

        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public string Course { get; set; }

        public byte Semester { get; set; }

        public bool IsYearLong { get; set; }

        public FacultyDto Faculty { get; set; }
    }
}
