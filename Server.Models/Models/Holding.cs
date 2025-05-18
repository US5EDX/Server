using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Holding
{
    [JsonIgnore]
    public short EduYear { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    [JsonIgnore]
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
