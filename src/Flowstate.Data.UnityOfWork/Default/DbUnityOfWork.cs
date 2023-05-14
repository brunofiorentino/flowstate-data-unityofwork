using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Default
{
    internal sealed class DbUnityOfWork<TDbConnection, TDbTransaction> : IUnityOfWork
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        private DbUnityOfWorkTransaction<TDbTransaction> _currentTransaction;

        public DbUnityOfWork(Func<TDbConnection> createDbConnection)
        {
            if (createDbConnection == null) throw new ArgumentNullException(nameof(createDbConnection));
            DbConnection = createDbConnection();
        }

        public TDbConnection DbConnection { get; }

        public DbUnityOfWorkTransaction<TDbTransaction> CurrentTransaction =>
            _currentTransaction is { Completed: false } ? _currentTransaction : null;

        public bool Completed { get; private set; }

        IUnityOfWorkTransaction IUnityOfWork.CurrentTransaction => CurrentTransaction;

        public (TDbConnection, TDbTransaction) DbConnectionDbTransactionPair() =>
            (EnsureOpenDbConnection(), CurrentTransaction?.DbTransaction);

        public async Task<(TDbConnection, TDbTransaction)> DbConnectionDbTransactionPairAsync
            (CancellationToken cancellationToken) =>
                (await EnsureOpenConnectionAsync(cancellationToken), CurrentTransaction?.DbTransaction);

        public DbUnityOfWorkTransaction<TDbTransaction> StartTransaction(IsolationLevel isolationLevel)
        {
            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");
        
            _currentTransaction = new DbUnityOfWorkTransaction<TDbTransaction>(
                ((TDbTransaction)EnsureOpenDbConnection().BeginTransaction(isolationLevel)));
        
            return _currentTransaction;
        }

        IUnityOfWorkTransaction IUnityOfWork.StartTransaction(IsolationLevel isolationLevel) =>
            StartTransaction(isolationLevel);

        public DbUnityOfWorkTransaction<TDbTransaction> StartTransaction() =>
            StartTransaction(IsolationLevel.ReadCommitted);

        IUnityOfWorkTransaction IUnityOfWork.StartTransaction() =>
            StartTransaction();

        public async Task<DbUnityOfWorkTransaction<TDbTransaction>> StartTransactionAsync(
            IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");
        
            _currentTransaction = new DbUnityOfWorkTransaction<TDbTransaction>(
                ((TDbTransaction)await (await EnsureOpenConnectionAsync(cancellationToken))
                    .BeginTransactionAsync(isolationLevel, cancellationToken)));
        
            return _currentTransaction;
        }

        async Task<IUnityOfWorkTransaction> IUnityOfWork.StartTransactionAsync(
            IsolationLevel isolationLevel, CancellationToken cancellationToken) =>
            await StartTransactionAsync(isolationLevel, cancellationToken);

        public async Task<DbUnityOfWorkTransaction<TDbTransaction>> StartTransactionAsync(
            CancellationToken cancellationToken) =>
            await StartTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        async Task<IUnityOfWorkTransaction> IUnityOfWork.StartTransactionAsync(
            CancellationToken cancellationToken) =>
            await StartTransactionAsync(cancellationToken);

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
        
        private TDbConnection EnsureOpenDbConnection()
        {
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            return DbConnection;
        }

        private async Task<TDbConnection> EnsureOpenConnectionAsync(CancellationToken cancellationToken)
        {
            if (DbConnection.State == ConnectionState.Closed) await DbConnection.OpenAsync(cancellationToken);
            return DbConnection;
        }
    }
}