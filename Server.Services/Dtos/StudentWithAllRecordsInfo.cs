namespace Server.Services.Dtos
{
    public class StudentWithAllRecordsInfo
    {
        public string FullName { get; set; }

        public IEnumerable<RecordDisciplineInfo> Records { get; set; }
    }

    public class RecordDisciplineInfo
    {
        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public short EduYear { get; set; }

        public byte Semester { get; set; }
    }
}
