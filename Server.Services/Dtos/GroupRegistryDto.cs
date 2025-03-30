using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class GroupRegistryDto
    {
        [Range(1, uint.MaxValue - 1)]
        public uint? GroupId { get; set; }

        [Required]
        [Length(1, 30)]
        public string GroupCode { get; set; }

        [Required]
        [Range(1, uint.MaxValue - 1)]
        public uint SpecialtyId { get; set; }

        [Required]
        [Range(1, 3)]
        public byte EduLevel { get; set; }

        [Required]
        [Range(0, 12)]
        public byte Course { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Nonparsemester { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Parsemester { get; set; }

        [Length(26, 26)]
        public string? CuratorId { get; set; }
    }
}
