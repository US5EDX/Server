using Microsoft.EntityFrameworkCore;
using Server.Data.Interceptors;
using Server.Models.Models;

namespace Server.Data.DbContexts;

public partial class ElCoursesDbContext(DbContextOptions<ElCoursesDbContext> options, AuditInterceptor interceptor) :
    DbContext(options)
{
    public virtual DbSet<AppSetting> AppSettings { get; set; }
    public virtual DbSet<Auditlog> Auditlogs { get; set; }
    public virtual DbSet<Discipline> Disciplines { get; set; }
    public virtual DbSet<Faculty> Faculties { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Holding> Holdings { get; set; }
    public virtual DbSet<Record> Records { get; set; }
    public virtual DbSet<Specialty> Specialties { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        base.OnConfiguring(optionsBuilder.AddInterceptors(interceptor));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4")
            .ApplyConfigurationsFromAssembly(typeof(ElCoursesDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
            return await base.SaveChangesAsync(cancellationToken);

        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        var result = await base.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return result;
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
