using Server.Models.Enums;
using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Group
{
    [JsonIgnore]
    public uint GroupId { get; set; }

    public string GroupCode { get; set; } = null!;

    /// <summary>
    /// 1 - bachelor
    /// 2 - master
    /// 3 - phd
    /// </summary>
    public EduLevels EduLevel { get; set; }

    public byte DurationOfStudy { get; set; }

    public short AdmissionYear { get; set; }

    public uint SpecialtyId { get; set; }

    /// <summary>
    /// disciplines count on non-par semester
    /// </summary>
    public byte Nonparsemester { get; set; }

    /// <summary>
    /// disciplines count on par semester
    /// </summary>
    public byte Parsemester { get; set; }

    /// <summary>
    /// When group must choose disciplines right after admission set to true
    /// </summary>
    public bool HasEnterChoise { get; set; }

    public byte ChoiceDifference { get; set; }

    public byte[]? CuratorId { get; set; }

    [JsonIgnore]
    public virtual Worker? Curator { get; set; }

    [JsonIgnore]
    public virtual Specialty Specialty { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
