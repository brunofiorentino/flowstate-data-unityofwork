using Flowstate.Data.UnityOfWork.Tests.UnitTests.Shared;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Common;

public sealed class WhenRelyingOnAsyncTransactions : CancellableTests
{
    [Fact]
    public async Task CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.BeginTransactionAsync(TestCancellationToken))
        {
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);
            await transaction.RollbackAsync(TestCancellationToken);
        }

        Assert.Empty(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CreatedEntityIsPersistedWhenCommitIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.BeginTransactionAsync(TestCancellationToken))
        {
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);
            await transaction.CommitAsync(TestCancellationToken);
        }

        Assert.Single(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CreatedEntityIsNoPersistedWhenCommitIsNotInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        await using (var transaction = await unityOfWork.BeginTransactionAsync(TestCancellationToken))
            await testRepository.AddAsync(new("1", "a"), TestCancellationToken);

        Assert.Empty(await testRepository.FindAllAsync(TestCancellationToken));
    }

    [Fact]
    public async Task CannotBeginAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();

        await using var transaction = await unityOfWork.BeginTransactionAsync(TestCancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(() => unityOfWork.BeginTransactionAsync(TestCancellationToken));
    }
}