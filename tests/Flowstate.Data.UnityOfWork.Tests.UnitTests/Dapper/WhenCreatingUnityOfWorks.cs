using Flowstate.Data.UnityOfWork.Dapper;
using Microsoft.Data.Sqlite;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Dapper;

public sealed class WhenCreatingUnityOfWorks
{
    [Fact]
    public void Members_are_properly_initilized()
    {
        using var connection = new SqliteConnection();
        var unityOfWork = new DapperUnityOfWork(connection);
        Assert.NotNull(unityOfWork.DbConnection);
        Assert.Same(connection, unityOfWork.DbConnection);

        Assert.Null(unityOfWork.CurrentTransaction);
        Assert.Null(((IUnityOfWork)unityOfWork).CurrentTransaction);
    }
}