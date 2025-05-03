using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class FacultyDto
    {
        [Required]
        [Range(1, uint.MaxValue - 1)]
        public uint FacultyId { get; set; }

        [Required]
        [Length(1, 100)]
        public string FacultyName { get; set; } = null!;
    }
}
