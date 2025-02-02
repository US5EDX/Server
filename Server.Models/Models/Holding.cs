using System;
using System.Collections.Generic;

namespace Server.Models.Models;

public partial class Holding
{
    public short EduYear { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
