using Server.Models.Enums;

namespace Server.Models.Models;

/// <summary>
/// student-discipline table
/// </summary>
public partial class Record
{
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

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual Holding HoldingNavigation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
