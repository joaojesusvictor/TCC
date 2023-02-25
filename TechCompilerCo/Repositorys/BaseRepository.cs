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
        private DbSession _db;

        private string _sqlEncript = "EXEC GER_EncriptarDecriptar_GET @Valor, @Acao";

        public BaseRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<Encript> EncriptDecriptAsync(string valor, string acao)
        {
            var p = new ParametrosTran()
            {
                Valor = valor,
                Acao = acao
            };

            using var conn = _db.Connection;
            Encript resultado = await conn.QueryFirstAsync<Encript>(_sqlEncript, p);

            return resultado;
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