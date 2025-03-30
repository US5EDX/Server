using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class SpecialtyDto
    {
        [Range(0, uint.MaxValue - 1)]
        public uint? SpecialtyId { get; set; }

        [Required]
        [Length(1, 255)]
        public string SpecialtyName { get; set; } = null!;

        [Range(1, uint.MaxValue - 1)]
        public uint? FacultyId { get; set; }
    }
}
