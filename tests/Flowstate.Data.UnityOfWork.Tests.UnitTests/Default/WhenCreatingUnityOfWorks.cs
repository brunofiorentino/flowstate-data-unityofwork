using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public sealed class WhenCreatingUnityOfWorks
{
    [Fact]
    public void MembersAreProperlyInitialized()
    {
        var connection = new SqliteConnection();
        var unityOfWork = new DbUnityOfWork<SqliteConnection, SqliteTransaction>(() => connection);

        Assert.NotNull(unityOfWork.DbConnection);
        Assert.Same(connection, unityOfWork.DbConnection);
        Assert.Equal(ConnectionState.Closed, unityOfWork.DbConnection.State);

        Assert.Null(unityOfWork.CurrentTransaction);
        Assert.Null(((IUnityOfWork)unityOfWork).CurrentTransaction);
    }
}