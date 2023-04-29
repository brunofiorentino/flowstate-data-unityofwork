using System;
using System.Data.Common;

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

        public IUnityOfWorkTransaction BeginTransaction()
        {
            if (CurrentTransaction != null)
                throw new InvalidOperationException("Previous transaction is not completed.");

            DbConnection.OpenIfNeeded();
            _currentTransaction = new DapperUnityOfWorkTransaction(DbConnection.BeginTransaction());
            return _currentTransaction;
        }
    }
}