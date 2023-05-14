using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Default
{
    public interface IDbUnityOfWorkManager<TDbConnection, TDbTransaction> :
        IUnityOfWorkManager,
        IDbUnityOfWorkContext<TDbConnection, TDbTransaction> 
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        
    }
}