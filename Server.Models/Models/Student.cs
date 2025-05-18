using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Student
{
    [JsonIgnore]
    public byte[] StudentId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public uint Group { get; set; }

    public uint Faculty { get; set; }

    public bool Headman { get; set; }

    [JsonIgnore]
    public virtual Faculty FacultyNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Group GroupNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();

    public virtual User User { get; set; } = null!;
}
