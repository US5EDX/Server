using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class GroupWithSpecialtyDto
    {
        public uint? GroupId { get; set; }

        [Required]
        [Length(1, 30)]
        public string GroupCode { get; set; }

        [Required]
        public SpecialtyDto Specialty { get; set; }

        [Required]
        [Range(1, 3)]
        public byte EduLevel { get; set; }

        [Required]
        [Range(1, 12)]
        public byte Course { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Nonparsemester { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Parsemester { get; set; }
    }
}
