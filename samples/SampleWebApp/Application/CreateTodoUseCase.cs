using Flowstate.Data.UnityOfWork;
using SampleWebApp.Domain;

namespace SampleWebApp.Application;

public class CreateTodoUseCase
{
    private readonly IUnityOfWorkManager _unityOfWorkManager;
    private readonly ITodoRepository _todoRepository;

    public CreateTodoUseCase(IUnityOfWorkManager unityOfWorkManager, ITodoRepository todoRepository)
    {
        _unityOfWorkManager = unityOfWorkManager;
        _todoRepository = todoRepository;
    }

    public async Task Execute(Todo todo, CancellationToken cancellationToken)
    {
        await using var unityOfWork = _unityOfWorkManager.StartUnityOfWork();
        await using var transaction = await unityOfWork.StartTransactionAsync(cancellationToken);
        await _todoRepository.AddAsync(todo, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}