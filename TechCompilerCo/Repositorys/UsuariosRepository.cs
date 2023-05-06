using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using static Azure.Core.HttpHeader;

namespace TechCompilerCo.Repositorys
{
    public class UsuariosRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Func_Usuarios_TRAN @Modo, @Login, @Senha, @UsuarioTran, @CodigoUsuario, @NomeUsuario, @Email, @UsuarioAdm, @CodigoFuncionario";

        public UsuariosRepository()
        {
            _db = new DbSession().SqlConnection();
        }       

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            IEnumerable<Usuario> results = new List<Usuario>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Usuario>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Usuario> GetUsuarioAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 5,
                CodigoUsuario = id
            };

            Usuario result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Usuario>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(UsuariosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoFuncionario = model.CodigoFuncionario,
                UsuarioTran = model.CodigoUsuarioLogado,
                Login = model.LoginUsuario,
                Senha = model.SetSenhaHash(),
                Email = model.Email,
                UsuarioAdm = model.UsuarioAdm
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(UsuariosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                UsuarioTran = model.CodigoUsuarioLogado,
                CodigoUsuario = model.CodigoUsuario,
                Login = model.LoginUsuario,
                Email = model.Email,
                UsuarioAdm = model.UsuarioAdm
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task DeleteAsync(int id, int codigoUsuarioLogado)
        {
            var p = new ParametrosTran()
            {
                Modo = 3,
                CodigoUsuario = id,
                UsuarioTran = codigoUsuarioLogado
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public async Task<bool> ValidaUsuarioLoginAsync(int codigoFuncionario)
        {
            var p = new ParametrosTran()
            {
                Modo = 10,
                CodigoFuncionario = codigoFuncionario
            };

            bool result = false;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<bool>(_sqlTran, p);
            }

            return result;
        }

        public async Task<bool> ValidaUsuarioLoginEditAsync(int codigoUsuario, int codigoFuncionario)
        {
            var p = new ParametrosTran()
            {
                Modo = 11,
                CodigoUsuario = codigoUsuario,
                CodigoFuncionario = codigoFuncionario
            };

            bool result = false;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<bool>(_sqlTran, p);
            }

            return result;
        }

        public class Usuario
        {
            public int CodigoUsuario { get; set; }
            public string? LoginUsuario { get; set; }
            public string? NomeUsuario { get; set; }
            public string? Email { get; set; }
            public bool UsuarioAdm { get; set; }
            public int CodigoFuncionario { get; set; }
            public DateTime DataInclusao { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public string? Login { get; set; }
            public string? Senha { get; set; }
            public int UsuarioTran { get; set; }
            public int CodigoUsuario { get; set; }
            public string? NomeUsuario { get; set; }
            public string? Email { get; set; }
            public bool UsuarioAdm { get; set; }
            public int CodigoFuncionario { get; set; }
        }
    }
}