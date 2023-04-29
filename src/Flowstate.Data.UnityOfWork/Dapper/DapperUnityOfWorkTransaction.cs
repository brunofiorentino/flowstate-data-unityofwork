using System;
using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Dapper
{
    public class DapperUnityOfWorkTransaction : IUnityOfWorkTransaction
    {
        public DapperUnityOfWorkTransaction(DbTransaction dbTransaction) =>
            DbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));

        public DbTransaction DbTransaction { get; }
        public bool Completed { get; private set; }

        public void Commit()
        {
            try
            {
                DbTransaction.Commit();
            }
            finally
            {
                Completed = true;
            }
        }

        public void Rollback()
        {
            try
            {
                DbTransaction.Rollback();
            }
            finally
            {
                Completed = true;
            }
        }

        public void Dispose()
        {
            try
            {
                DbTransaction.Dispose();
            }
            finally
            {
                Completed = true;
            }
        }
    }
}