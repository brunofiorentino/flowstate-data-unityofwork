using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWork: IDisposable, IAsyncDisposable
    {
        IUnityOfWorkTransaction CurrentTransaction { get; }
        IUnityOfWorkTransaction BeginTransaction();
        IUnityOfWorkTransaction BeginTransaction(IsolationLevel isolationLevel);
        Task<IUnityOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task<IUnityOfWorkTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
    }
}