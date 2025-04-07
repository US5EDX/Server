namespace Server.Services.Dtos
{
    public class StudentYearsRecordsDto
    {
        public short Holding { get; set; }

        public byte Semester { get; set; }

        public bool Approved { get; set; }

        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }
    }
}
