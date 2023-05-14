using Dapper;
using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;
using SampleWebApp.Domain;

namespace SampleWebApp.Infrastructure.Domain;

public class TodoRepository : ITodoRepository
{
    private readonly IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> _dbUnityOfWorkContext;

    public TodoRepository(IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> dbUnityOfWorkContext) =>
        _dbUnityOfWorkContext = dbUnityOfWorkContext;
    
    public async Task AddAsync(Todo todo, CancellationToken cancellationToken)
    {
        var (dbConnection, dbTransaction) = await _dbUnityOfWorkContext.GetDbObjectsAsync(cancellationToken);
        const string sql = "INSERT Todos(Id, Name, IsComplete) VALUES (@Id, @Name, @IsComplete);";
        await dbConnection.ExecuteAsync(sql, todo, dbTransaction);
    }
}