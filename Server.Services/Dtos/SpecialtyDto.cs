using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class SpecialtyDto
    {
        [Required]
        public uint SpecialtyId { get; set; }

        [Required]
        [Length(1, 255)]
        public string SpecialtyName { get; set; } = null!;

        [Required]
        public uint FacultyId { get; set; }
    }
}
