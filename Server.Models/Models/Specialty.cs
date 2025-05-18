using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Specialty
{
    [JsonIgnore]
    public uint SpecialtyId { get; set; }

    public string SpecialtyName { get; set; } = null!;

    public uint FacultyId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    [JsonIgnore]
    public virtual Faculty Faculty { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
