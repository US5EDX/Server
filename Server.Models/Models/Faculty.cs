namespace Server.Models.Models;

public partial class Faculty
{
    public uint FacultyId { get; set; }

    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
