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
using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class VendasRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Ven_Vendas_TRAN @Modo, @CodigoVenda, @CodigoProduto, @CodigoCliente, @Quantidade, @Valor, @DataVenda, @UsuarioTran";

        public VendasRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<Venda>> GetVendasAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Venda> results = new List<Venda>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Venda>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Venda> GetVendaAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoVenda = id
            };

            Venda result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Venda>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(VendasViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoProduto = model.CodigoProduto,
                CodigoCliente= model.CodigoCliente,
                Quantidade = model.Quantidade,
                Valor = model.Valor,
                DataVenda = model.DataVenda,
                UsuarioTran = model.CodigoUsuario
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(VendasViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoVenda = model.CodigoVenda,
                CodigoProduto = model.CodigoProduto,
                CodigoCliente = model.CodigoCliente,
                Quantidade = model.Quantidade,
                Valor = model.Valor,
                DataVenda = model.DataVenda,
                UsuarioTran = model.CodigoUsuario
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
                CodigoVenda = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class Venda
        {
            public int CodigoVenda { get; set; }
            public int CodigoProduto { get; set; }
            public int CodigoCliente { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataVenda { get; set; }
            public bool Ativo { get; set; }
            public string? NomeCliente { get; set; }
            public string? DescricaoProduto { get; set; }
            public DateTime DataInclusao { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoVenda { get; set; }
            public int CodigoProduto { get; set; }
            public int CodigoCliente { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
            public DateTime? DataVenda { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}