# Flowstate.Data.UnityOfWork

Flowstate.Data.UnityOfWork is a straightforward, partial C# Unity of Work library compatible with any standard .NET data provider ([System.Data.Common](https://learn.microsoft.com/en-us/dotnet/api/system.data.common?view=net-6.0) classes), exposing managed, shared, provider typed [DbConnection](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection?view=net-6.0) and [DbTransaction](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbtransaction?view=net-6.0) instances for repositories. 

It contributes to organize your your application when you're looking for simpler designs, playing with raw .NET data providers, .NET minimal apis and/or compatibility with forthcoming native AOT support.


## Usage

### Dependency Injection Setup
``` 
// Add scoped DbUnityOfWorkManagerFactory for chosen .NET data provider.
services.AddScoped(_ =>
    DbUnityOfWorkManagerFactory.Create<SqliteConnection, SqliteTransaction>(() =>
        new("Data Source=InMemorySample;Mode=Memory;Cache=Shared")));

// Add higher-level interface for application layer consumption.
services.AddScoped<IUnityOfWorkManager>(p => 
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());

// Add specific lower-level interface for repository consumption.
services.AddScoped<IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction>>(p =>
    p.GetRequiredService<IDbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>());


// Add repository components...
services.AddScoped<ITodoRepository, TodoRepository>();

// Add application layer components...
services.AddScoped<CreateTodoUseCase>();

```
**NOTES:** 

- All required registrations from the library need [**Scoped Lifetime**](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#scoped), hence dependent services too.
- As is common with database-related libraries, "non guaranteed thread-safety for instance members" is propagated from .NET data providers. Given this limitation, if your usage is different from ASP.NET (where DI scopes are implicitly managed and bound to web requests), say a job, and you need parallelism, then you need to define and manage DI scopes associated with your threads.
    - We're investigating ways to add optional/convenient DI scope management for these cases.

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
        
        // await _someOtherRepository.Get...
        await _todoRepository.AddAsync(todo, cancellationToken);
        
        // Commit for happy path relying on IDisposable otherwise.
        await transaction.CommitAsync(cancellationToken);
    }
}
```


### Infrastructure Layer / Repositories

``` 
public class TodoRepository : ITodoRepository
{
    private readonly IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> _dbUnityOfWorkContext;

    public TodoRepository(IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> dbUnityOfWorkContext) =>
        _dbUnityOfWorkContext = dbUnityOfWorkContext;
    
    public async Task AddAsync(Todo todo, CancellationToken cancellationToken)
    {
        // Get managed, shared db objects.
        var (dbConnection, dbTransaction) = await _dbUnityOfWorkContext.GetDbObjectsAsync(cancellationToken); 

        // Just use them without lifecycle and success/failure handling concerns at this layer.
        const string sql = "INSERT Todos(Id, Name, IsComplete) VALUES (@Id, @Name, @IsComplete);";
        await dbConnection.ExecuteAsync(sql, todo, dbTransaction);
    }
}
```
