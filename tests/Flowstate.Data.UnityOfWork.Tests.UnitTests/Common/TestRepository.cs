using Flowstate.Data.UnityOfWork.Common;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Common;

public class TestRepository
{
    private readonly IContextualDbObjects<SqliteConnection, SqliteTransaction> _contextualDbObjects;

    public TestRepository(IContextualDbObjects<SqliteConnection, SqliteTransaction> contextualDbObjects) =>
        _contextualDbObjects = contextualDbObjects;


    public void InitializeSchema()
    {
        var (connection, _) = _contextualDbObjects.Get();
        connection.Execute("CREATE TABLE test (id TEXT, name TEXT);");
    }

    public void Add(TestEntity testEntity)
    {
        var (connection, transaction) = _contextualDbObjects.Get();

        const string sql = "INSERT INTO test (id, name) VALUES (@Id, @Name);";
        var affectedRows = connection.Execute(sql, param: testEntity, transaction: transaction);

        EnsureRowsWereAffected(1, affectedRows);
    }

    public async Task AddAsync(TestEntity testEntity, CancellationToken cancellationToken)
    {
        var (connection, transaction) = await _contextualDbObjects.GetAsync(cancellationToken);

        const string sql = "INSERT INTO test (id, name) VALUES (@Id, @Name);";
        var affectedRows = await connection.ExecuteAsync(sql, param: testEntity, transaction: transaction);

        EnsureRowsWereAffected(1, affectedRows);
    }

    public List<TestEntity> FindAll()
    {
        var (connection, transaction) = _contextualDbObjects.Get();

        const string sql = "SELECT t.id, t.name FROM test t ORDER BY t.name;";
        var testEntities = connection.Query<TestEntity>(sql, transaction: transaction).ToList();

        return testEntities;
    }

    public async Task<List<TestEntity>> FindAllAsync(CancellationToken cancellationToken)
    {
        var (connection, transaction) = await _contextualDbObjects.GetAsync(cancellationToken);

        const string sql = "SELECT t.id, t.name FROM test t ORDER BY t.name;";
        var testEntities = connection.Query<TestEntity>(sql, transaction: transaction).ToList();

        return testEntities;
    }

    public static (IUnityOfWorkContext, TestRepository) GivenUnityOfWorkContextAndTestRepositorySetup()
    {
        var unityOfWorkContext = new CommonUnityOfWorkContext<SqliteConnection, SqliteTransaction>(
            () => new("Data Source=:memory:"));

        var testRepository = new TestRepository(unityOfWorkContext);

        return (unityOfWorkContext, testRepository);
    }

    private static void EnsureRowsWereAffected(int expected, int actual)
    {
        if (expected != actual) throw new Exception($"Unexpected number of rows affected: {actual}");
    }
}