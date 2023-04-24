namespace Flowstate.Data;

public interface IUnityOfWorkTransaction : IDisposable
{
    Task Commit(CancellationToken cancellationToken = default);
    void Rollback();
}