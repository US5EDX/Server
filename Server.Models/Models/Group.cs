using System;
using System.Collections.Generic;

namespace Server.Models.Models;

public partial class Group
{
    public uint GroupId { get; set; }

    public string GroupCode { get; set; } = null!;

    /// <summary>
    /// 1 - bachelor
    /// 2 - master
    /// 3 - phd
    /// </summary>
    public byte EduLevel { get; set; }

    public byte Course { get; set; }

    public uint SpecialtyId { get; set; }

    /// <summary>
    /// disciplines count on non-par semester
    /// </summary>
    public byte Nonparsemester { get; set; }

    /// <summary>
    /// disciplines count on par semester
    /// </summary>
    public byte Parsemester { get; set; }

    public byte[]? CuratorId { get; set; }

    public virtual Worker? Curator { get; set; }

    public virtual Specialty Specialty { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
