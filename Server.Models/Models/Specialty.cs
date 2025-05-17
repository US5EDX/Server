namespace Server.Models.Models;

public partial class Specialty
{
    public uint SpecialtyId { get; set; }

    public string SpecialtyName { get; set; } = null!;

    public uint FacultyId { get; set; }

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
