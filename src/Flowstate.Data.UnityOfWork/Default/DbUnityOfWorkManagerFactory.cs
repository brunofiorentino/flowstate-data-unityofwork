using System;
using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Default
{
    public static class DbUnityOfWorkManagerFactory
    {
        public static IDbUnityOfWorkManager<TDbConnection, TDbTransaction> Create<TDbConnection, TDbTransaction>(
            Func<TDbConnection> createConnection)
            where TDbConnection : DbConnection
            where TDbTransaction : DbTransaction =>
            new DbUnityOfWorkManager<TDbConnection, TDbTransaction>(createConnection);
    }
}