namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWork
    {
        IUnityOfWorkTransaction CurrentTransaction { get; }
        IUnityOfWorkTransaction BeginTransaction();
    }
}