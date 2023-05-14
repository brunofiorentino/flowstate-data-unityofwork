namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Default;

public class WhenStartingDbUnitiesOfWork
{
    [Fact]
    public async Task CannotStartAnotherUnityOfWorkBeforeCompletingPrevious()
    {
        var (unityOfWorkManager, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();

        await using var unityOfWork = unityOfWorkManager.StartUnityOfWork();
        Assert.Throws<InvalidOperationException>(() => unityOfWorkManager.StartUnityOfWork());
    }

    [Fact]
    public async Task CanStartAndCompleteMultipleUnitiesOfWorkSequentiallyFromTheSameManager()
    {
        var (unityOfWorkManager, _) = TestRepository.GivenUnityOfWorkManagerAndTestRepositorySetup();

        for (var i = 0; i < 3; i++)
            await using (var unityOfWorkA = unityOfWorkManager.StartUnityOfWork()) ;
    }
}