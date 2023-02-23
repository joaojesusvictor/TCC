using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCompilerCo.Repositorys
{
    public interface IRepository
    {
        DbConnection GetConnection();

        T Get<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        List<T> GetAll<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        int Execute<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        T Insert<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        T Update<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);
    }
}