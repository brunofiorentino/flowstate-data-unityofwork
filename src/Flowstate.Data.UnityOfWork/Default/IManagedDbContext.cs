using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Default
{
    public interface IManagedDbContext<TDbConnection, TDbTransaction>
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        (TDbConnection, TDbTransaction) GetDbObjects();
        Task<(TDbConnection, TDbTransaction)> GetDbObjectsAsync(CancellationToken cancellationToken);
    }
}