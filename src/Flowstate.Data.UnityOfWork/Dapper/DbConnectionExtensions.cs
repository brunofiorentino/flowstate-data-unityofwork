using System.Data;
using System.Data.Common;

namespace Flowstate.Data.UnityOfWork.Dapper
{
    public static class DbConnectionExtensions
    {
        public static void OpenIfNeeded(this DbConnection @this)
        {
            if (@this.State == ConnectionState.Open) return;
            @this.Open();
        }
    }
}