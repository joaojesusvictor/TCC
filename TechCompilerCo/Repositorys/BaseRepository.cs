using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class BaseRepository
    {
        private readonly IDbConnection _db;
        private string _sqlEncript = "EXEC GER_EncriptarDecriptar_GET @Valor, @Acao";

        public BaseRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<Encript> EncriptDecriptAsync(string valor, string acao)
        {
            var p = new ParametrosTran()
            {
                Valor = valor,
                Acao = acao
            };

            Encript result = new Encript();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Encript>(_sqlEncript, p);
            }

            return result;
        }

        public class Combo
        {
            public string Id { get; set; }
            public string Nome { get; set; }
        }

        public class Encript
        {
            public string? Resultado { get; set; }
        }

        private class ParametrosTran
        {
            public string? Valor { get; set; }
            public string? Acao { get; set; }
        }
    }
}