using Flowstate.Data.UnityOfWork.Default;
using Microsoft.Data.Sqlite;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public class WhenCreatingDbUnityOfWorkManagers
{
    [Fact]
    public void CreatesNonNullInstanceWithExpectedUnderlyingType()
    {
        var unityOfWorkManager = DbUnityOfWorkManagerFactory
            .Create<SqliteConnection, SqliteTransaction>(() => new SqliteConnection());
        
        Assert.NotNull(unityOfWorkManager);
        Assert.IsType<DbUnityOfWorkManager<SqliteConnection, SqliteTransaction>>(unityOfWorkManager);
    }
}