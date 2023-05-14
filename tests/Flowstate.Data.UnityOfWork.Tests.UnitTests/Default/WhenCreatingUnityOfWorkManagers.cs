using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public class WhenCreatingUnityOfWorkManagers
{
    [Fact]
    public void CreatesNonNullInstanceWithExpectedUnderlyingType()
    {
        var dbUnityOfWorkManager = DbUnityOfWorkManagerFactory
            .Create<SqliteConnection, SqliteTransaction>(() => new SqliteConnection());
        
        Assert.NotNull(dbUnityOfWorkManager);
        Assert.IsType<DbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>(dbUnityOfWorkManager);
    }
}