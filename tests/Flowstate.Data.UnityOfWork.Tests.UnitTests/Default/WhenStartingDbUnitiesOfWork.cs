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
#pragma warning disable CS0642 // Possible mistaken empty statement
            await using (var unityOfWorkA = unityOfWorkManager.StartUnityOfWork()) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
    }
}