using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Dapper
{
    public class DapperUnityOfWork : IUnityOfWork
    {
        private DapperUnityOfWorkTransaction _currentTransaction;

        public DapperUnityOfWork(DbConnection connection) =>
            DbConnection = connection ?? throw new ArgumentNullException(nameof(connection));

        public DbConnection DbConnection { get; }

        public DapperUnityOfWorkTransaction CurrentTransaction =>
            _currentTransaction != null && !_currentTransaction.Completed ? _currentTransaction : null;

        IUnityOfWorkTransaction IUnityOfWork.CurrentTransaction => CurrentTransaction;

        public IUnityOfWorkTransaction BeginTransaction(IsolationLevel isolationLevel)
        {

            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");

            DbConnection.OpenIfNeeded();
            _currentTransaction = new DapperUnityOfWorkTransaction(DbConnection.BeginTransaction(isolationLevel));
            return _currentTransaction;
        }

        public IUnityOfWorkTransaction BeginTransaction() =>
            BeginTransaction(IsolationLevel.ReadCommitted);


        public async Task<IUnityOfWorkTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {

            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");

            await DbConnection.OpenIfNeededAsync(cancellationToken);
            _currentTransaction = new DapperUnityOfWorkTransaction(
                await DbConnection.BeginTransactionAsync(isolationLevel, cancellationToken));

            return _currentTransaction;
        }

        public async Task<IUnityOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
            await BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }
}