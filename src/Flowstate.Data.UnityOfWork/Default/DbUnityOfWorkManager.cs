using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Default
{
    internal sealed class DbUnityOfWorkManager<TDbConnection, TDbTransaction> :
        IDbUnityOfWorkManager<TDbConnection, TDbTransaction>
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        private readonly Func<TDbConnection> _createConnection;
        private DbUnityOfWork<TDbConnection, TDbTransaction> _currentUnityOfWork;

        public DbUnityOfWorkManager(Func<TDbConnection> createConnection) =>
            _createConnection = createConnection ?? throw new ArgumentNullException(nameof(createConnection));

        public DbUnityOfWork<TDbConnection, TDbTransaction> CurrentUnityOfWork =>
            _currentUnityOfWork is { Completed: false } ? _currentUnityOfWork : null;

        IUnityOfWork IUnityOfWorkManager.CurrentUnityOfWork => CurrentUnityOfWork;

        public DbUnityOfWork<TDbConnection, TDbTransaction> StartUnityOfWork()
        {
            if (CurrentUnityOfWork != null)
                throw new InvalidOperationException("Previous contextual unity of work is not completed.");

            _currentUnityOfWork = new DbUnityOfWork<TDbConnection, TDbTransaction>(_createConnection);
            return _currentUnityOfWork;
        }

        IUnityOfWork IUnityOfWorkManager.StartUnityOfWork() => StartUnityOfWork();

        internal const string CannotUseDbUnityOfWorkContextOperationsWithoutStartingOuterUnityOfWorkMessage = 
            "Cannot use DbUnityOfWorkContext operations without starting outer UnityOfWork.";

        (TDbConnection, TDbTransaction) IDbUnityOfWorkContext<TDbConnection, TDbTransaction>.GetDbObjects() 
        {
            if (CurrentUnityOfWork == null)
                throw new InvalidOperationException(
                    CannotUseDbUnityOfWorkContextOperationsWithoutStartingOuterUnityOfWorkMessage);

            return CurrentUnityOfWork.GetDbObjects(); 
        }

        async Task<(TDbConnection, TDbTransaction)>
            IDbUnityOfWorkContext<TDbConnection, TDbTransaction>.GetDbObjectsAsync(CancellationToken cancellationToken)
        {
            if (CurrentUnityOfWork == null)
                throw new InvalidOperationException(
                    CannotUseDbUnityOfWorkContextOperationsWithoutStartingOuterUnityOfWorkMessage);
            
            return await CurrentUnityOfWork.GetDbObjectsAsync(cancellationToken);
        }
    }
}