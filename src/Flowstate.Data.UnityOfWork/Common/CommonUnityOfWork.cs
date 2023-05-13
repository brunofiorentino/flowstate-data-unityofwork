using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Common
{
    public sealed class CommonUnityOfWork<TConnection, TTransaction> : IUnityOfWork
        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        private CommonUnityOfWorkTransaction<TTransaction> _currentTransaction;

        public CommonUnityOfWork(Func<TConnection> createConnection)
        {
            if (createConnection == null) throw new ArgumentNullException(nameof(createConnection));
            DbConnection = createConnection();
        }

        public TConnection DbConnection { get; }

        public CommonUnityOfWorkTransaction<TTransaction> CurrentTransaction =>
            _currentTransaction != null && !_currentTransaction.Completed ? _currentTransaction : null;

        public bool Completed { get; private set; }

        IUnityOfWorkTransaction IUnityOfWork.CurrentTransaction => CurrentTransaction;

        public TConnection EnsureOpenDbConnection()
        {
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            return DbConnection;
        }

        public async Task<TConnection> EnsureOpenConnectionAsync(CancellationToken cancellationToken)
        {
            if (DbConnection.State == ConnectionState.Closed) await DbConnection.OpenAsync(cancellationToken);
            return DbConnection;
        }

        public (TConnection, TTransaction) DbConnectionDbTransactionPair() =>
            (EnsureOpenDbConnection(), CurrentTransaction?.DbTransaction);


        public async Task<(TConnection, TTransaction)> DbConnectionDbTransactionPairAsync
            (CancellationToken cancellationToken) =>
                (await EnsureOpenConnectionAsync(cancellationToken), CurrentTransaction?.DbTransaction);


        public IUnityOfWorkTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");

            _currentTransaction = new CommonUnityOfWorkTransaction<TTransaction>(
                ((TTransaction)EnsureOpenDbConnection().BeginTransaction(isolationLevel)));

            return _currentTransaction;
        }

        public IUnityOfWorkTransaction BeginTransaction() =>
            BeginTransaction(IsolationLevel.ReadCommitted);


        public async Task<IUnityOfWorkTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");

            _currentTransaction = new CommonUnityOfWorkTransaction<TTransaction>(
                ((TTransaction)await (await EnsureOpenConnectionAsync(cancellationToken))
                    .BeginTransactionAsync(isolationLevel, cancellationToken)));

            return _currentTransaction;
        }

        public async Task<IUnityOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
            await BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        public void Dispose()
        {
            Completed = true;
            DbConnection.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            Completed = true;
            await DbConnection.DisposeAsync();
        }
    }
}