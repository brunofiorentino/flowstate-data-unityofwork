namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public sealed class WhenRelyingOnSyncTransactions
{
    [Fact]
    public void CreatedEntityIsNotPersistedWhenRollbackIsInvoked()
    {
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
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
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
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
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
        testRepository.InitializeSchema();

        using (var transaction = unityOfWork.StartTransaction())
            testRepository.Add(new("1", "a"));

        Assert.Empty(testRepository.FindAll());
    }

    [Fact]
    public void CannotStartAnotherTransactionBeforeCompletingPrevious()
    {
        var (unityOfWorkManager, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        using var unityOfWork = unityOfWorkManager.StartUnityOfWork();

        using var transaction = unityOfWork.StartTransaction();        

        Assert.Throws<InvalidOperationException>(() => unityOfWork.StartTransaction());
    }
}