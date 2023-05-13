namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Common;

public sealed class WhenRelyingOnSyncTransactions
{
    [Fact]
    public void CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.BeginTransaction())
        {
            testRepository.Add(new("1", "a"));
            transaction.Rollback();
        }

        Assert.Empty(testRepository.FindAll());
    }

    [Fact]
    public void CreatedEntityIsPersistedWhenCommitIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.BeginTransaction())
        {
            testRepository.Add(new("1", "a"));
            transaction.Commit();
        }

        Assert.Single(testRepository.FindAll());
    }

    [Fact]
    public void CreatedEntityIsNoPersistedWhenCommitIsNotInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.BeginTransaction())
            testRepository.Add(new("1", "a"));

        Assert.Empty(testRepository.FindAll());
    }

    [Fact]
    public void CannotBeginAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkContextAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.CreateContextUnityOfWork();

        using var transaction = unityOfWork.BeginTransaction();        

        Assert.Throws<InvalidOperationException>(() => unityOfWork.BeginTransaction());
    }
}