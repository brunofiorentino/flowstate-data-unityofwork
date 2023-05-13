using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Common
{
    public interface IContextualDbObjects<TConnection, TTransaction>
        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        (TConnection, TTransaction) Get();
        Task<(TConnection, TTransaction)> GetAsync(CancellationToken cancellationToken);
    }
}