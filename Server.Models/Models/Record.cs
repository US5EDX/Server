using Server.Models.Enums;
using System.Text.Json.Serialization;

namespace Server.Models.Models;

/// <summary>
/// student-discipline table
/// </summary>
public partial class Record
{
    [JsonIgnore]
    public uint RecordId { get; set; }

    public byte[] StudentId { get; set; } = null!;

    public uint DisciplineId { get; set; }

    /// <summary>
    /// 0 - both
    /// 1 - non-pair
    /// 2 - pair
    /// </summary>
    public Semesters Semester { get; set; }

    public short Holding { get; set; }

    public RecordStatus Approved { get; set; }

    [JsonIgnore]
    public virtual Discipline Discipline { get; set; } = null!;

    [JsonIgnore]
    public virtual Holding HoldingNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Student Student { get; set; } = null!;
}
