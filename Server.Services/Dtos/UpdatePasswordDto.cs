using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        [Length(26, 26)]
        public string UserId { get; set; }

        [Required]
        [MinLength(1)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(1)]
        public string NewPassword { get; set; }
    }
}
