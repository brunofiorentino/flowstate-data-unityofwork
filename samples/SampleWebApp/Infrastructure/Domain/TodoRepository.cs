using Dapper;
using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;
using SampleWebApp.Domain;

namespace SampleWebApp.Infrastructure.Domain;

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