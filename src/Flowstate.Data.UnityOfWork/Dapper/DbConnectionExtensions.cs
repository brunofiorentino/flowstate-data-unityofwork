using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Flowstate.Data.UnityOfWork.Dapper
{
    public static class DbConnectionExtensions
    {
        public static void OpenIfNeeded(this DbConnection @this)
        {
            if (@this.State == ConnectionState.Open) return;
            @this.Open();
        }
        public static async Task OpenIfNeededAsync(this DbConnection @this, CancellationToken cancellationToken)
        {
            if (@this.State == ConnectionState.Open) return;
            await @this.OpenAsync(cancellationToken);
        }
    }
}