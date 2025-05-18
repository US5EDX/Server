using Server.Models.Enums;
using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class Discipline
{
    [JsonIgnore]
    public uint DisciplineId { get; set; }

    public string DisciplineCode { get; set; } = null!;

    /// <summary>
    /// 1 - USC
    /// 2 - FSC
    /// </summary>
    public CatalogTypes CatalogType { get; set; }

    public uint FacultyId { get; set; }

    public uint? SpecialtyId { get; set; }

    public string DisciplineName { get; set; } = null!;

    /// <summary>
    /// 1 - bachelor
    /// 2 - master
    /// 3 - phd
    /// </summary>
    public EduLevels EduLevel { get; set; }

    public byte Course { get; set; }

    /// <summary>
    /// 0 - both
    /// 1 - non-pair
    /// 2 - pair
    /// </summary>
    public Semesters Semester { get; set; }

    public string Prerequisites { get; set; } = null!;

    /// <summary>
    /// why it is interesting
    /// </summary>
    public string Interest { get; set; } = null!;

    /// <summary>
    /// max count of students assinged to discipline
    /// </summary>
    public int MaxCount { get; set; }

    /// <summary>
    /// min count of students assinged to discipline
    /// </summary>
    public int MinCount { get; set; }

    public string Url { get; set; } = null!;

    public short Holding { get; set; }

    public bool IsYearLong { get; set; }

    public bool IsOpen { get; set; }

    public byte[] CreatorId { get; set; } = null!;

    [JsonIgnore]
    public virtual Worker Creator { get; set; } = null!;

    [JsonIgnore]
    public virtual Faculty Faculty { get; set; } = null!;

    [JsonIgnore]
    public virtual Holding HoldingNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();

    [JsonIgnore]
    public virtual Specialty? Specialty { get; set; }
}
