using Microsoft.EntityFrameworkCore;
using Template.DataAccess.Entities;

namespace Template.DataAccess;

public class AppDbContext : DbContext
{
    private readonly AuditInterceptor _auditInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    public DbSet<CategoryEntity> Categories { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());
        foreach (var foreignKey in cascadeFKs) foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
        base.OnModelCreating(modelBuilder);
    }
}