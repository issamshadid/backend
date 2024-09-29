namespace Template.DataAccess;

public interface IAppDbContextFactory<out TContext>
    where TContext : AppDbContext
{
    TContext Create();
}