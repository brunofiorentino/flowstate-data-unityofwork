namespace Flowstate.Data.UnityOfWork
{
    public interface IUnityOfWorkContext
    {
        IUnityOfWork CurrentUnityOfWork { get; }
        IUnityOfWork CreateContextUnityOfWork();
    }
}