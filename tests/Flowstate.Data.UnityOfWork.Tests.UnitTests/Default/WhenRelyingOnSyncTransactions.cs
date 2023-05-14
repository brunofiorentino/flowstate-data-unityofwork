namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public sealed class WhenRelyingOnSyncTransactions
{
    [Fact]
    public void CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.StartTransaction())
        {
            testRepository.Add(new("1", "a"));
            transaction.Rollback();
        }

        Assert.Empty(testRepository.FindAll());
    }

    [Fact]
    public void CreatedEntityIsPersistedWhenCommitIsInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.StartTransaction())
        {
            testRepository.Add(new("1", "a"));
            transaction.Commit();
        }

        Assert.Single(testRepository.FindAll());
    }

    [Fact]
    public void CreatedEntityIsNotPersistedWhenCommitIsNotInvoked()
    {
        var (unityOfWorkContext, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.StartUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.StartTransaction())
            testRepository.Add(new("1", "a"));

        Assert.Empty(testRepository.FindAll());
    }

    [Fact]
    public void CannotStartAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkContext, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkContext.StartUnityOfWork();

        using var transaction = unityOfWork.StartTransaction();        

        Assert.Throws<InvalidOperationException>(() => unityOfWork.StartTransaction());
    }
}