namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWorkManager
    {
        IUnityOfWork CurrentUnityOfWork { get; }
        IUnityOfWork StartUnityOfWork();
    }
}