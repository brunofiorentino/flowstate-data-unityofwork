using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Default
{
    public interface IDbUnityOfWorkManager<TDbConnection, TDbTransaction> :
        IUnityOfWorkManager,
        IManagedDbContext<TDbConnection, TDbTransaction> 
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        
    }
}