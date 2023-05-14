using Flowstate.Data.UnityOfWork.Tests.UnitTests.Shared;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public sealed class WhenRelyingOnAsyncTransactions : CancellableTests
{
    [Fact]
    public async Task CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
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
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
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
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken))
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);

        Assert.Empty(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CannotStartAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkManager, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();

        await using var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(() => unityOfWork.StartTransactionAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CanStartAndCompleteMultipleTransactionsSequentiallyFromTheSameUnityOfWork()
    {
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        const int iterations = 3;
        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
        testRepository.InitializeSchema();

        for (var i = 0; i < iterations; i++)
        {
            await using (var transaction = await unityOfWork.StartTransactionAsync(TestCancellationToken))
            {
                await testRepository.AddAsync(new($"t{i}", "..."), TestCancellationToken);
                await transaction.CommitAsync(TestCancellationToken);
            }
        }

        Assert.Equal(iterations, (await testRepository.FindAllAsync(TestCancellationToken)).Count);
    }
}