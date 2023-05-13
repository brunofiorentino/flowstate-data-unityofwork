using Flowstate.Data.UnityOfWork.Common;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Common;

public sealed class WhenCreatingUnityOfWorks
{
    [Fact]
    public void MembersAreProperlyInitilized()
    {
        var connection = new SqliteConnection();
        var unityOfWork = new CommonUnityOfWork<SqliteConnection, SqliteTransaction>(() => connection);

        Assert.NotNull(unityOfWork.DbConnection);
        Assert.Same(connection, unityOfWork.DbConnection);
        Assert.Equal(ConnectionState.Closed, unityOfWork.DbConnection.State);

        Assert.Null(unityOfWork.CurrentTransaction);
        Assert.Null(((IUnityOfWork)unityOfWork).CurrentTransaction);
    }
}