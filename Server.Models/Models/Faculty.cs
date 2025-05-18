using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Faculty
{
    public uint FacultyId { get; set; }

    public string FacultyName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    [JsonIgnore]
    public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();

    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [JsonIgnore]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
