using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using Dapper;

namespace TechCompilerCo.Repositorys
{
    public class BaseRepository
    {
        protected string _connectionString;

        private string _sqlEncript = "";

        public BaseRepository()
        {

        }

        //protected async Task<T> UsarSql<T>(Func<IDbConnection, Task<T>> getData)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            var data = await getData(connection);

        //            return data;
        //        }
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
        //    }
        //}

        //public async Task<Encript> EncriptDecriptAsync(string valor, string acao)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 4,
        //        Valor = valor,
        //        Acao = acao
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.QueryFirstOrDefaultAsync<Encript>(_sqlEncript, p);
        //    });
        //}

        public class Encript
        {

        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public string Valor { get; set; }
            public string Acao { get; set; }
        }
    }
}