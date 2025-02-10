using Server.Services.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class HoldingDto
    {
        [Required]
        [Range(1901, 2155)]
        public short EduYear { get; set; }

        [Required]
        [CompareDateLessThan(nameof(EndDate))]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }
    }
}
