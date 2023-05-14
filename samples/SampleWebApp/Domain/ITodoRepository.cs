namespace SampleWebApp.Domain;

public interface ITodoRepository
{
    Task AddAsync(Todo todo, CancellationToken cancellationToken);
}