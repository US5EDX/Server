using System.ComponentModel.DataAnnotations;

namespace Server.Services.Dtos
{
    public class WorkerShortInfoDto
    {
        [Required]
        [Length(26, 26)]
        public string WorkerId { get; set; }

        [Required]
        [Length(1, 150)]
        public string FullName { get; set; }

        public WorkerShortInfoDto() { }

        public WorkerShortInfoDto(byte[] id, string fullName)
        {
            WorkerId = new Ulid(id).ToString();
            FullName = fullName;
        }
    }
}
