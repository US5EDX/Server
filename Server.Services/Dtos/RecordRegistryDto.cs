using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class RecordRegistryWithoutStudent
    {
        [Range(1, uint.MaxValue - 1)]
        public uint? RecordId { get; set; }

        [Required]
        [Range(1, uint.MaxValue - 1)]
        public uint DisciplineId { get; set; }

        [Required]
        [Range(1, 2)]
        public byte Semester { get; set; }

        [Required]
        [Range(2020, 2155)]
        public short Holding { get; set; }
    }

    public class RecordRegistryDto : RecordRegistryWithoutStudent
    {
        [Required]
        [Length(26, 26)]
        public string StudentId { get; set; }
    }
}
