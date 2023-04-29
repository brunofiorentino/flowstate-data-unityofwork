using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWorkTransaction : IDisposable, IAsyncDisposable
    {
        bool Completed { get; }
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}