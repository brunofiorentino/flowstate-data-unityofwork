using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWork : IDisposable, IAsyncDisposable
    {
        IUnityOfWorkTransaction CurrentTransaction { get; }
        IUnityOfWorkTransaction StartTransaction();
        IUnityOfWorkTransaction StartTransaction(IsolationLevel isolationLevel);
        Task<IUnityOfWorkTransaction> StartTransactionAsync(CancellationToken cancellationToken);

        Task<IUnityOfWorkTransaction> StartTransactionAsync(
            IsolationLevel isolationLevel, CancellationToken cancellationToken);
    }
}