using Dapper;
using Flowstate.Data.UnityOfWork.Dapper;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Dapper;

public sealed class WhenBeginningAsyncTransactions : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested) _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    [Fact]
    public async Task Created_record_is_not_persisted_when_Rollback_is_invoked()
    {
        using var connection = await GivenTestDbObjectsSetupAsync(_cancellationTokenSource.Token);
        var unityOfWork = new DapperUnityOfWork(connection);

        await using (var transaction = await unityOfWork.BeginTransactionAsync(_cancellationTokenSource.Token))
        {
            await InsertTestRecordAsync(connection, _cancellationTokenSource.Token);
            await transaction.RollbackAsync(_cancellationTokenSource.Token);
        }

        Assert.Empty(await QueryAllTestRecordsAsync(connection, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task Created_record_is_persisted_when_Commit_is_invoked()
    {
        using var connection = await GivenTestDbObjectsSetupAsync(_cancellationTokenSource.Token);
        var unityOfWork = new DapperUnityOfWork(connection);

        await using (var transaction = await unityOfWork.BeginTransactionAsync(_cancellationTokenSource.Token))
        {
            await InsertTestRecordAsync(connection, _cancellationTokenSource.Token);
            await transaction.CommitAsync(_cancellationTokenSource.Token);
        }

        Assert.Single(await QueryAllTestRecordsAsync(connection, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task Created_record_is_not_persisted_when_Commit_is_not_invoked()
    {
        using var connection = await GivenTestDbObjectsSetupAsync(_cancellationTokenSource.Token);
        var unityOfWork = new DapperUnityOfWork(connection);

        await using (var transaction = await unityOfWork.BeginTransactionAsync(_cancellationTokenSource.Token))
            await InsertTestRecordAsync(connection, _cancellationTokenSource.Token);

        Assert.Empty(await QueryAllTestRecordsAsync(connection, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task Cannot_begin_another_transaction_before_completing_a_previous_one()
    {
        using var connection = new SqliteConnection();
        var unityOfWork = new DapperUnityOfWork(connection);
        await using var transaction = await unityOfWork.BeginTransactionAsync(_cancellationTokenSource.Token);
        Assert.Throws<InvalidOperationException>(() => unityOfWork.BeginTransaction());
    }

    private async Task<DbConnection> GivenTestDbObjectsSetupAsync(CancellationToken cancellationToken)
    {
        var connection = new SqliteConnection("Data Source=:memory:;");

        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync("CREATE TABLE test (id TEXT, name TEXT);", cancellationToken);
        return connection;
    }

    private async Task InsertTestRecordAsync(DbConnection connection, CancellationToken cancellationToken) =>
        await connection.ExecuteAsync("INSERT INTO test (id, name) VALUES (1, 'n1');", cancellationToken);

    private async Task<IEnumerable<dynamic>> QueryAllTestRecordsAsync(
        DbConnection connection, CancellationToken cancellationToken) =>
        (await connection.QueryAsync<dynamic>("SELECT * FROM test;")).ToList();
}