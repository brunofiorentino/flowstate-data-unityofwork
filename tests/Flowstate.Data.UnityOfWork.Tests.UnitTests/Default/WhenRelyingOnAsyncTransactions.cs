using Flowstate.Data.UnityOfWork.Tests.UnitTests.Shared;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public sealed class WhenRelyingOnAsyncTransactions : CancellableTests
{
    [Fact]
    public async Task CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken))
        {
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);
            await transaction.RollbackAsync(TestCancellationToken);
        }

        Assert.Empty(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CreatedEntityIsPersistedWhenCommitIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken))
        {
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);
            await transaction.CommitAsync(TestCancellationToken);
        }

        Assert.Single(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CreatedEntityIsNotPersistedWhenCommitIsNotInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken))
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);

        Assert.Empty(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CannotStartAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkContext, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkContext.StartUnityOfWork();

        await using var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(() => unityOfWork.StartTransactionAsync(TestCancellationToken));
    }
}