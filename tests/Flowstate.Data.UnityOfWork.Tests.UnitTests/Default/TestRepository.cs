using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

internal class TestRepository
{
    private readonly IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> _dbUnityOfWorkContext;

    public TestRepository(IDbUnityOfWorkContext<SqliteConnection, SqliteTransaction> dbUnityOfWorkContext) =>
        _dbUnityOfWorkContext = dbUnityOfWorkContext;

    public void InitializeSchema()
    {
        var (dbConnection, _) = _dbUnityOfWorkContext.GetDbObjects();
        dbConnection.Execute("CREATE TABLE test (id TEXT, name TEXT);");
    }

    public void Add(TestEntity testEntity)
    {
        var (dbConnection, dbTransaction) = _dbUnityOfWorkContext.GetDbObjects();

        const string sql = "INSERT INTO test (id, name) VALUES (@Id, @Name);";
        var affectedRows = dbConnection.Execute(sql, param: testEntity, transaction: dbTransaction);

        EnsureRowsWereAffected(1, affectedRows);
    }

    public async Task AddAsync(TestEntity testEntity, CancellationToken cancellationToken)
    {
        var (dbConnection, dbTransaction) = await _dbUnityOfWorkContext.GetDbObjectsAsync(cancellationToken);

        const string sql = "INSERT INTO test (id, name) VALUES (@Id, @Name);";
        var affectedRows = await dbConnection.ExecuteAsync(sql, param: testEntity, transaction: dbTransaction);

        EnsureRowsWereAffected(1, affectedRows);
    }

    public List<TestEntity> FindAll()
    {
        var (dbConnection, dbTransaction) = _dbUnityOfWorkContext.GetDbObjects();

        const string sql = "SELECT t.id, t.name FROM test t ORDER BY t.name;";
        var testEntities = dbConnection.Query<TestEntity>(sql, transaction: dbTransaction).ToList();

        return testEntities;
    }

    public async Task<List<TestEntity>> FindAllAsync(CancellationToken cancellationToken)
    {
        var (dbConnection, dbTransaction) = await _dbUnityOfWorkContext.GetDbObjectsAsync(cancellationToken);

        const string sql = "SELECT t.id, t.name FROM test t ORDER BY t.name;";
        var testEntities = dbConnection.Query<TestEntity>(sql, transaction: dbTransaction).ToList();

        return testEntities;
    }

    public static (IUnityOfWorkManager, TestRepository) GivenUnityOfWorkManagerAndTestRepositorySetup()
    {
        var unityOfWorkManager = DbUnityOfWorkManagerFactory.Create<SqliteConnection,SqliteTransaction>(
            () => new("Data Source=:memory:"));
    
        var testRepository = new TestRepository(unityOfWorkManager);
    
        return (unityOfWorkManager, testRepository);
    }

    private static void EnsureRowsWereAffected(int expected, int actual)
    {
        if (expected != actual) throw new Exception($"Unexpected number of rows affected: {actual}");
    }
}