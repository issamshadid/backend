namespace Template.DataAccess.Repositories;

public interface ICorrelateBy<TIdentifier>
{
    TIdentifier Id { get; set; }
}