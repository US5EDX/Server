using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        [Length(16, 16)]
        public byte[] UserId { get; set; }

        [Required]
        [MinLength(1)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(1)]
        public string NewPassword { get; set; }
    }
}
