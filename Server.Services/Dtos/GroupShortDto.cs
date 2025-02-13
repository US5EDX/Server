using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class GroupShortDto
    {
        [Required]
        [Range(1, uint.MaxValue - 1)]
        public uint GroupId { get; set; }

        public string GroupCode { get; set; } = null!;
    }
}
