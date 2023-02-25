using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class LoginRepository
    {
        private DbSession _db;
        private string _sqlTran = "EXEC Func_Usuarios_TRAN @Modo, @Login, @Senha";

        public LoginRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<bool> GetValidacaoAsync(string login, string senha)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                Login = login,
                Senha = senha
            };

            using var conn = _db.Connection;
            bool result = await conn.QueryFirstAsync<bool>(_sqlTran, p);

            return result;
        }

        public class Login
        {
            public string? Usuario { get; set; }
            public string? Senha { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public string? Login { get; set; }
            public string? Senha { get; set; }
        }
    }
}