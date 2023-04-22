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
    public class ContasReceberRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Cre_ContasReceber_TRAN @Modo, @CodigoCre, @CodigoCliente, @NumeroDocumento, @DataVencimento, @Valor, @DataPagamento, @FormaPagamento, @ServicoCobrado, @ContaRecebida, @UsuarioTran";

        public ContasReceberRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<ContaReceber>> GetContasRecebidasAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<ContaReceber> results = new List<ContaReceber>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<ContaReceber>(_sqlTran, p);
            }

            return results;
        }

        public async Task<IEnumerable<ContaReceber>> GetContasNaoRecebidasAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            IEnumerable<ContaReceber> results = new List<ContaReceber>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<ContaReceber>(_sqlTran, p);
            }

            return results;
        }

        public async Task<ContaReceber> GetContaReceberAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoCre = id
            };

            ContaReceber result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<ContaReceber>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(ContasReceberViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                UsuarioTran = model.CodigoUsuario,
                CodigoCliente = model.CodigoCliente,
                NumeroDocumento = model.NumeroDocumento,
                DataVencimento = model.DataVencimento,
                Valor = model.Valor,
                DataPagamento = model.DataPagamento,
                FormaPagamento = model.FormaPagamento,
                ServicoCobrado = model.ServicoCobrado,
                ContaRecebida = model.DataPagamento != null
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(ContasReceberViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                UsuarioTran = model.CodigoUsuario,
                CodigoCre = model.CodigoCre,
                CodigoCliente = model.CodigoCliente,
                NumeroDocumento = model.NumeroDocumento,
                DataVencimento = model.DataVencimento,
                Valor = model.Valor,
                DataPagamento = model.DataPagamento,
                FormaPagamento = model.FormaPagamento,
                ServicoCobrado = model.ServicoCobrado,
                ContaRecebida = model.DataPagamento != null
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task DeleteAsync(int id, int codigoUsuario)
        {
            var p = new ParametrosTran()
            {
                Modo = 3,
                CodigoCre = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class ContaReceber
        {
            public int CodigoCre { get; set; }
            public int CodigoCliente { get; set; }
            public string? NumeroDocumento { get; set; }
            public DateTime DataVencimento { get; set; }
            public decimal Valor { get; set; }
            public DateTime? DataPagamento { get; set; }
            public string? FormaPagamento { get; set; }
            public string? ServicoCobrado { get; set; }
            public bool Recebida { get; set; }
            public bool Ativo { get; set; }
            public DateTime DataInclusao { get; set; }
            public string? NomeCliente { get; set; }
            public string? Documento { get; set; }
            public string? Telefone1 { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoCre { get; set; }
            public int CodigoCliente { get; set; }
            public string? NumeroDocumento { get; set; }
            public DateTime? DataVencimento { get; set; }
            public decimal Valor { get; set; }
            public DateTime? DataPagamento { get; set; }
            public string? FormaPagamento { get; set; }
            public string? ServicoCobrado { get; set; }
            public bool ContaRecebida { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}