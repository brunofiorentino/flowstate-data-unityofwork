using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Common
{

    public sealed class CommonUnityOfWorkContext<TConnection, TTransaction>
        : IUnityOfWorkContext, IContextualDbObjects<TConnection, TTransaction>

        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        private readonly Func<TConnection> _createConnection;
        private CommonUnityOfWork<TConnection, TTransaction> _currentUnityOfWork;

        public CommonUnityOfWorkContext(Func<TConnection> createConnection) =>
            _createConnection = createConnection ?? throw new ArgumentNullException(nameof(createConnection));

        public CommonUnityOfWork<TConnection, TTransaction> CurrentUnityOfWork =>
            _currentUnityOfWork != null && !_currentUnityOfWork.Completed ? _currentUnityOfWork : null;

        IUnityOfWork IUnityOfWorkContext.CurrentUnityOfWork => CurrentUnityOfWork;

        public CommonUnityOfWork<TConnection, TTransaction> CreateContextualUnityOfWork()
        {
            if (CurrentUnityOfWork != null)
                throw new InvalidOperationException("Previous contextual unity of work is not completed.");

            _currentUnityOfWork = new CommonUnityOfWork<TConnection, TTransaction>(_createConnection);
            return _currentUnityOfWork;
        }

        IUnityOfWork IUnityOfWorkContext.CreateContextUnityOfWork() => CreateContextualUnityOfWork();

        (TConnection, TTransaction) IContextualDbObjects<TConnection, TTransaction>.Get() =>
            CurrentUnityOfWork!.DbConnectionDbTransactionPair();

        async Task<(TConnection, TTransaction)>
            IContextualDbObjects<TConnection, TTransaction>.GetAsync(CancellationToken cancellationToken) =>
                await CurrentUnityOfWork!.DbConnectionDbTransactionPairAsync(cancellationToken);
    }
}