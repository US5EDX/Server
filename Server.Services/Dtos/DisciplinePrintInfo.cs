namespace Server.Services.Dtos
{
    public class DisciplinePrintInfo
    {
        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public int StudentsCount { get; set; }

        public string? SpecialtyName { get; set; }

        public byte EduLevel { get; set; }

        public string Course { get; set; }

        public byte Semester { get; set; }

        public int MinCount { get; set; }

        public int MaxCount { get; set; }

        public bool IsOpen { get; set; }

        public string ColorStatus { get; set; }
    }
}
