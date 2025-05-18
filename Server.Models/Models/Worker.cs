using System.Text.Json.Serialization;

namespace Server.Models.Models;

/// <summary>
/// lecturer and admin info table
/// </summary>
public partial class Worker
{
    [JsonIgnore]
    public byte[] WorkerId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public uint Faculty { get; set; }

    public string Department { get; set; } = null!;

    public string Position { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    [JsonIgnore]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [JsonIgnore]
    public virtual Faculty FacultyNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
