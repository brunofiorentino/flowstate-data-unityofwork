using System.Data;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWork
    {
        IUnityOfWorkTransaction CurrentTransaction { get; }
        IUnityOfWorkTransaction BeginTransaction();
        IUnityOfWorkTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}