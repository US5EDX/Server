namespace Server.Services.Dtos
{
    public class RecordShortDisciplineInfoDto
    {
        public uint RecordId { get; set; }

        public byte ChosenSemester { get; set; }

        public byte Approved { get; set; }

        public uint DisciplineId { get; set; }

        public string DisciplineCode { get; set; }

        public string DisciplineName { get; set; }

        public bool IsYearLong { get; set; }
    }

    public class RecordWithDisciplineInfoDto : RecordShortDisciplineInfoDto
    {
        public string Course { get; set; }

        public byte EduLevel { get; set; }

        public byte Semester { get; set; }

        public int SubscribersCount { get; set; }

        public bool IsOpen { get; set; }
    }
}
