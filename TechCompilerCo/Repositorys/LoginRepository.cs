using System;
using System.Collections.Generic;
using System.Data;
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
        //private DbSession _db;
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Func_Usuarios_TRAN @Modo, @Login, @Senha, @UsuarioTran";

        public LoginRepository(/*DbSession dbSession*/)
        {
            //_db = dbSession;
            _db = new DbSession().SqlConnection();
        }

        public async Task<UsuarioViewModel> GetUsuarioAsync(string login)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                Login = login
            };

            //using var conn = _db.Connection;
            //UsuarioViewModel result = await conn.QueryFirstOrDefaultAsync<UsuarioViewModel>(_sqlTran, p);

            //return result;

            //using (var conn = _db.Connection)
            //{
            //    UsuarioViewModel result = await conn.QueryFirstOrDefaultAsync<UsuarioViewModel>(_sqlTran, p);
            //    return result;
            //}

            UsuarioViewModel result = new UsuarioViewModel();

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<UsuarioViewModel>(_sqlTran, p);
                _db.Dispose();
            }

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
            public string? UsuarioTran { get; set; }
        }
    }
}