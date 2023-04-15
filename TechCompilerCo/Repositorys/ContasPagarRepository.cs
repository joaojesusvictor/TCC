using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class ContasPagarRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Cpa_ContasPagar_TRAN @Modo, @CodigoCpa, @CodigoFuncionario, @Valor, @DataVencimento, @DataPagamento, @ServicoCobrado, @ContaPaga, @UsuarioTran";

        public ContasPagarRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<ContaPagar>> GetContasPagasAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<ContaPagar> results = new List<ContaPagar>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<ContaPagar>(_sqlTran, p);
            }

            return results;
        }

        public async Task<IEnumerable<ContaPagar>> GetContasNaoPagasAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            IEnumerable<ContaPagar> results = new List<ContaPagar>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<ContaPagar>(_sqlTran, p);
            }

            return results;
        }

        public async Task<ContaPagar> GetContaPagarAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoCpa = id
            };

            ContaPagar result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<ContaPagar>(_sqlTran, p);
            }

            return result;
        }

        public async Task CreateAsync(ContasPagarViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                UsuarioTran = model.CodigoUsuario,
                CodigoFuncionario = model.CodigoFuncionario,
                Valor = model.Valor,
                DataVencimento = model.DataVencimento,
                DataPagamento = model.DataPagamento,
                ServicoCobrado = model.ServicoCobrado,
                ContaPaga = model.DataPagamento != null
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public async Task UpdateAsync(ContasPagarViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                UsuarioTran = model.CodigoUsuario,
                CodigoCpa= model.CodigoCpa,
                CodigoFuncionario = model.CodigoFuncionario,
                Valor = model.Valor,
                DataVencimento = model.DataVencimento,
                DataPagamento = model.DataPagamento,
                ServicoCobrado = model.ServicoCobrado,
                ContaPaga = model.DataPagamento != null
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
                CodigoCpa = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class ContaPagar
        {
            public int CodigoCpa { get; set; }
            public int CodigoFuncionario { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataVencimento { get; set; }
            public DateTime? DataPagamento { get; set; }
            public string? ServicoCobrado { get; set; }
            public bool Paga { get; set; }
            public bool Ativo { get; set; }
            public DateTime DataInclusao { get; set; }
            public string? NomeFuncionario { get; set; }
            public string? Cpf { get; set; }
            public string? Telefone1 { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoCpa { get; set; }
            public int CodigoFuncionario { get; set; }
            public decimal Valor { get; set; }
            public DateTime? DataVencimento { get; set; }
            public DateTime? DataPagamento { get; set; }
            public string? ServicoCobrado { get; set; }
            public bool ContaPaga { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}