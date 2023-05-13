namespace Flowstate.Data.UnityOfWork.Tests.UnitTests.Shared
{
    /// <summary> Support for cancellable, responsive tests</summary>
    public class CancellableTests : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        protected CancellationToken TestCancellationToken => _cancellationTokenSource.Token;

        public void Dispose()
        {
            if (!_cancellationTokenSource.IsCancellationRequested) _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}