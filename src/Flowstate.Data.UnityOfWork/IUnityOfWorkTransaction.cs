using System;

namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWorkTransaction : IDisposable
    {
        bool Completed { get; }
        void Commit();
        void Rollback();
    }
}