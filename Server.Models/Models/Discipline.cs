using System;
using System.Collections.Generic;

namespace Server.Models.Models;

public partial class Discipline
{
    public uint DisciplineId { get; set; }

    public string DisciplineCode { get; set; } = null!;

    /// <summary>
    /// 1 - USC
    /// 2 - FSC
    /// </summary>
    public byte CatalogType { get; set; }

    public uint FacultyId { get; set; }

    public uint? SpecialtyId { get; set; }

    public string DisciplineName { get; set; } = null!;

    /// <summary>
    /// 1 - bachelor
    /// 2 - master
    /// 3 - phd
    /// </summary>
    public byte EduLevel { get; set; }

    public string Course { get; set; } = null!;

    /// <summary>
    /// 0 - both
    /// 1 - non-pair
    /// 2 - pair
    /// </summary>
    public byte Semester { get; set; }

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

    public int SubscribersCount { get; set; }

    public short Holding { get; set; }

    public bool IsOpen { get; set; }

    public byte[] CreatorId { get; set; } = null!;

    public virtual Worker Creator { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual Holding HoldingNavigation { get; set; } = null!;

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();

    public virtual Specialty? Specialty { get; set; }
}
