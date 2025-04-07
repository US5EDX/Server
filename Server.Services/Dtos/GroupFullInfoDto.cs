namespace Server.Services.Dtos
{
    public class GroupFullInfoDto
    {
        public uint? GroupId { get; set; }

        public string GroupCode { get; set; }

        public SpecialtyDto Specialty { get; set; }

        public byte EduLevel { get; set; }

        public byte Course { get; set; }

        public byte DurationOfStudy { get; set; }

        public short AdmissionYear { get; set; }

        public byte Nonparsemester { get; set; }

        public byte Parsemester { get; set; }

        public bool HasEnterChoise { get; set; }

        public WorkerShortInfoDto? CuratorInfo { get; set; }
    }
}
