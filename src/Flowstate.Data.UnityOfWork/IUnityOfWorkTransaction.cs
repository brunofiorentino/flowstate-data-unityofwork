using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWorkTransaction : IDisposable, IAsyncDisposable
    {
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}