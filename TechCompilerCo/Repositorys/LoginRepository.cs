using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Func_Usuarios_TRAN @Modo, @Login, @Senha, @UsuarioTran, @CodigoUsuario, @NomeUsuario, @Email, @UsuarioAdm, @CodigoFuncionario, @NovaSenha";

        public LoginRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<UsuarioLogadoViewModel> GetUsuarioAsync(string login)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                Login = login
            };

            UsuarioLogadoViewModel result = new UsuarioLogadoViewModel();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<UsuarioLogadoViewModel>(_sqlTran, p);
            }

            return result;
        }

        public async Task<UsuarioLogadoViewModel> BuscarUsuarioRedefinirSenhaAsync(string login, string email)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                Login = login,
                Email = email
            };

            UsuarioLogadoViewModel result = new UsuarioLogadoViewModel();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<UsuarioLogadoViewModel>(_sqlTran, p);
            }

            return result;
        }

        public async Task AtualizaSenha(int codigoUsuario, string senha)
        {
            var p = new ParametrosTran()
            {
                Modo = 8,
                CodigoUsuario = codigoUsuario,
                Senha = senha
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            };
        }

        public async Task<bool> RedefineSenha(int codigoUsuario, string senhaAtual, string novaSenha)
        {
            var p = new ParametrosTran()
            {
                Modo = 9,
                CodigoUsuario = codigoUsuario,
                Senha = senhaAtual,
                NovaSenha= novaSenha
            };

            bool result = false;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<bool>(_sqlTran, p);
            }

            return result;
        }

        public class Login
        {
            public string? Usuario { get; set; }
            public string? Senha { get; set; }
            public string? Email { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public string? Login { get; set; }
            public string? Senha { get; set; }
            public string? NovaSenha { get; set; }
            public string? Email { get; set; }
            public string? UsuarioTran { get; set; }
            public int CodigoUsuario { get; set; }
            public string? NomeUsuario { get; set; }
            public bool UsuarioAdm { get; set; }
            public int CodigoFuncionario { get; set; }
        }
    }
}