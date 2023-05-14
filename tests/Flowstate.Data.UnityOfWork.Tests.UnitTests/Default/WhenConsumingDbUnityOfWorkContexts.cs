using Flowstate.Data.UnityOfWork.Tests.UnitTests.Shared;

namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public class WhenConsumingDbUnityOfWorkContexts : CancellableTests
{
    [Fact]
    public void CannotUseUnityOfWorkContextOperationsWithoutStartingOuterUnityOfWork()
    {
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        Assert.Throws<InvalidOperationException>(() =>
            testRepository.Add(new("1", "...")));
    }
    
    [Fact]
    public async Task CannotUseUnityOfWorkContextOperationsWithoutStartingOuterUnityOfWorkAsyncVersion()
    {
        var (unityOfWorkManager, testRepository) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            testRepository.AddAsync(new("1", "..."), TestCancellationToken));
    }
}
