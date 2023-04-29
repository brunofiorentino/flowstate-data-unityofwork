using Dapper;
using Flowstate.Data.UnityOfWork.Dapper;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Dapper;

public sealed class WhenBeginningSyncTransactions
{
    [Fact]
    public void Created_record_is_not_persisted_when_Rollback_is_invoked()
    {
        using var connection = GivenTestDbObjectsSetup();
        var unityOfWork = new DapperUnityOfWork(connection);

        using (var transaction = unityOfWork.BeginTransaction())
        {
            InsertTestRecord(connection);
            transaction.Rollback();
        }

        Assert.Empty(QueryAllTestRecords(connection));
    }

    [Fact]
    public void Created_record_is_persisted_when_Commit_is_invoked()
    {
        using var connection = GivenTestDbObjectsSetup();
        var unityOfWork = new DapperUnityOfWork(connection);

        using (var transaction = unityOfWork.BeginTransaction())
        {
            InsertTestRecord(connection);
            transaction.Commit();
        }

        Assert.Single(QueryAllTestRecords(connection));
    }

    [Fact]
    public void Created_record_is_not_persisted_when_Commit_is_not_invoked()
    {
        using var connection = GivenTestDbObjectsSetup();
        var unityOfWork = new DapperUnityOfWork(connection);

        using (var transaction = unityOfWork.BeginTransaction())
            InsertTestRecord(connection);

        Assert.Empty(QueryAllTestRecords(connection));
    }

    [Fact]
    public void Cannot_begin_another_transaction_before_completing_a_previous_one()
    {
        using var connection = new SqliteConnection();
        var unityOfWork = new DapperUnityOfWork(connection);
        using var transaction = unityOfWork.BeginTransaction();
        Assert.Throws<InvalidOperationException>(() => unityOfWork.BeginTransaction());
    }

    private static DbConnection GivenTestDbObjectsSetup()
    {
        var connection = new SqliteConnection("Data Source=:memory:;");

        connection.Open();
        connection.Execute("CREATE TABLE test (id TEXT, name TEXT);");
        return connection;
    }

    private static void InsertTestRecord(DbConnection connection) =>
        connection.Execute("INSERT INTO test (id, name) VALUES (1, 'n1');");

    private static IEnumerable<dynamic> QueryAllTestRecords(DbConnection connection) =>
        connection.Query<dynamic>("SELECT * FROM test;").ToList();
}