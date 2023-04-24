namespace Flowstate.Data;

public interface IUnityOfWork
{
    IUnityOfWorkTransaction CurrentTransaction { get; }
    Task<IUnityOfWorkTransaction> BeginTransaction(CancellationToken cancellationToken = default);
}