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
    public class GerarOsRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC OS_OrdemServico_TRAN @Modo, @CodigoOs, @CodigoCliente, @CodigoFuncionario, @DataInicio, @DataPrevisaoTermino, @DescricaoProblema, @Valor, @StatusOs, @UsuarioTran";

        public GerarOsRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<OrdemServico>> GetOrdensServicosAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<OrdemServico> results = new List<OrdemServico>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<OrdemServico>(_sqlTran, p);
            }

            return results;
        }

        public async Task<OrdemServico> GetOrdemServicoAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoOs = id
            };

            OrdemServico result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<OrdemServico>(_sqlTran, p);
            }

            return result;
        }

        public async Task CreateAsync(GerarOsViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoCliente = model.CodigoCliente,
                CodigoFuncionario = model.CodigoFuncionario,
                DataInicio = model.DataInicio,
                DataPrevisaoTermino = model.DataPrevisaoTermino,
                DescricaoProblema = model.DescricaoProblema,
                Valor = model.Valor,
                StatusOs = model.StatusOs,
                UsuarioTran = model.CodigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public async Task UpdateAsync(GerarOsViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoOs = model.CodigoOs,
                CodigoCliente = model.CodigoCliente,
                CodigoFuncionario = model.CodigoFuncionario,
                DataInicio = model.DataInicio,
                DataPrevisaoTermino = model.DataPrevisaoTermino,
                DescricaoProblema = model.DescricaoProblema,
                Valor = model.Valor,
                StatusOs = model.StatusOs,
                UsuarioTran = model.CodigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public async Task DeleteAsync(int id, int codigoUsuario)
        {
            var p = new ParametrosTran()
            {
                Modo = 3,
                CodigoOs = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class OrdemServico
        {
            public int CodigoOs { get; set; }
            public int CodigoCliente { get; set; }
            public int CodigoFuncionario { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataPrevisaoTermino { get; set; }
            public string? DescricaoProblema { get; set; }
            public decimal Valor { get; set; }
            public string? StatusOs { get; set; }
            public string? NomeCliente { get; set; }
            public string? Documento { get; set; }
            public string? Telefone1 { get; set; }
            public DateTime DataInclusao { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoOs { get; set; }
            public int CodigoCliente { get; set; }
            public int CodigoFuncionario { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataPrevisaoTermino { get; set; }
            public string? DescricaoProblema { get; set; }
            public decimal Valor { get; set; }
            public string? StatusOs { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}