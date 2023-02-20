using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace TechCompilerCo.Repositorys
{
    public class LoginRepository : BaseRepository
    {
        private string _sqlTran = "";

        public LoginRepository()
        {

        }

        //public async Task<Login> GetFuncionarioAsync(string login, string senha)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 4,
        //        Login = login,
        //        Senha = senha
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.QueryFirstOrDefaultAsync<Login>(_sqlTran, p);
        //    });
        //}

        public class Login
        {
            public string? Usuario { get; set; }        //Reclama se a prop nao é anulavel
            public string? Senha { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public string Login { get; set; }
            public string Senha { get; set; }
        }
    }
}