using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class AniversariantesRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC REL_Aniversariantes_TRAN @Modo, @Mes, @UsuarioTran";

        public AniversariantesRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<Aniversariante>> GetAniversariantesAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Aniversariante> results = new List<Aniversariante>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Aniversariante>(_sqlTran, p);
            }

            return results;
        }

        public async Task<IEnumerable<Aniversariante>> GetAniversarianteAsync(int mes)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                Mes = mes
            };

            IEnumerable<Aniversariante> results = new List<Aniversariante>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Aniversariante>(_sqlTran, p);
            }

            return results;
        }

        public class Aniversariante
        {
            public int QtdAniversariantes { get; set; }
            public int MesNascimento { get; set; }
            public string? Nome { get; set; }
            public string? Telefone { get; set; }
            public string? Email { get; set; }
            public int Idade { get; set; }
            public string? TipoPessoa { get; set; }
            public DateTime DataNascimento { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int Mes { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}