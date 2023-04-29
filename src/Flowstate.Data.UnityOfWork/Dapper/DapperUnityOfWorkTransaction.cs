﻿using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Dapper
{
    public sealed class DapperUnityOfWorkTransaction : IUnityOfWorkTransaction
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

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                await DbTransaction.CommitAsync(cancellationToken);
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

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            try
            {
                await DbTransaction.RollbackAsync(cancellationToken);
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

        public async ValueTask DisposeAsync()
        {
            try
            {
                await DbTransaction.DisposeAsync();
            }
            finally
            {
                Completed = true;
            }
        }
    }
}