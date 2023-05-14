# Flowstate.Data.UnityOfWork

Flowstate.Data.UnityOfWork is C# library that provides abstractions to manage an unit of work instance from the application layer while multiple repository instances access shared, contextual, provider specific persistence components. It includes a default implementation based on `System.Data.Common` classes, compatible with any standard .NET data provider, conveniently exposing managed connection and transaction instances for repositories.

## Usage


### Dependency Injection Setup

``` 
// Add scoped unity of work context for chosen DB provider
services.AddScoped(_ =>
    DbUnityOfWorkManagerFactory.Create<SqliteConnection, SqliteTransaction>(() =>
        new("Data Source=InMemorySample;Mode=Memory;Cache=Shared")));

// Add interface for repository consumption
services.AddScoped<IManagedDbContext<SqliteConnection, SqliteTransaction>>(p => 
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());

// Add interface for app layer consumption
services.AddScoped<IUnityOfWorkManager>(p => 
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());


// Repositories
services.AddScoped<ITodoRepository, TodoRepository>();

// Application Layer 
services.AddScoped<CreateTodoUseCase>();

```


### Application Layer

``` 
public class CreateTodoUseCase
{
    private readonly IUnityOfWorkManager _unityOfWorkManager;
    private readonly ITodoRepository _todoRepository;

    public CreateTodoUseCase(IUnityOfWorkManager unityOfWorkManager, ITodoRepository todoRepository)
    {
        _unityOfWorkManager = unityOfWorkManager;
        _todoRepository = todoRepository;
    }

    public async Task Execute(Todo todo, CancellationToken cancellationToken)
    {
        await using var unityOfWork = _unityOfWorkManager.StartUnityOfWork();
        await using var transaction = await unityOfWork.StartTransactionAsync(cancellationToken);
        await _todoRepository.AddAsync(todo, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
```


### Infrastructure Layer / Repositories

``` 
public class TodoRepository : ITodoRepository
{
    private readonly IManagedDbContext<SqliteConnection, SqliteTransaction> _managedDbContext;

    public TodoRepository(IManagedDbContext<SqliteConnection, SqliteTransaction> managedDbContext) =>
        _managedDbContext = managedDbContext;

    
    public async Task AddAsync(Todo todo, CancellationToken cancellationToken)
    {
        var (dbConnection, dbTransaction) = await _managedDbContext.GetDbObjectsAsync(cancellationToken);
        const string sql = "INSERT Todos(Id, Name, IsComplete) VALUES (@Id, @Name, @IsComplete);";
        await dbConnection.ExecuteAsync(sql, todo, dbTransaction);
    }
}

```
