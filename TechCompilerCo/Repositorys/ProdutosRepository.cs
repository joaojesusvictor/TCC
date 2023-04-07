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
using Microsoft.AspNetCore.Mvc.Rendering;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class ProdutosRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Prod_Produtos_TRAN @Modo, @CodigoProduto, @Descricao, @Referencia, @Localizacao, @Marca, @Categoria, @ValorUnitario, @Quantidade, @CodigoFornecedor, @UsuarioTran";

        public ProdutosRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<SelectListItem>> ComboProdutosAsync(bool addBranco = false)
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            var result = await _db.QueryAsync<Produto>(_sqlTran, p);

            var combo = new List<SelectListItem>();

            foreach (var r in result)
                combo.Add(new SelectListItem() { Value = r.CodigoProduto.ToString(), Text = r.Descricao });

            if (addBranco)
                combo.Insert(0, new SelectListItem());

            return combo;
        }

        public async Task<IEnumerable<Produto>> GetProdutosAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Produto> results = new List<Produto>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Produto>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Produto> GetProdutoAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoProduto = id
            };

            Produto result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Produto>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(ProdutosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                Referencia = model.Referencia,
                Descricao = model.Descricao,
                Localizacao = model.Localizacao,
                Marca = model.Marca,
                Categoria = model.Categoria,
                ValorUnitario = model.ValorUnitario,
                Quantidade = model.Quantidade,
                UsuarioTran = model.CodigoUsuario,
                CodigoFornecedor = model.CodigoFornecedor
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(ProdutosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoProduto = model.CodigoProduto,
                Referencia = model.Referencia,
                Descricao = model.Descricao,
                Localizacao = model.Localizacao,
                Marca = model.Marca,
                Categoria = model.Categoria,
                ValorUnitario = model.ValorUnitario,
                Quantidade = model.Quantidade,
                UsuarioTran = model.CodigoUsuario,
                CodigoFornecedor = model.CodigoFornecedor
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id, int codigoUsuario)
        {
            var p = new ParametrosTran()
            {
                Modo = 3,
                CodigoProduto = id,
                UsuarioTran = codigoUsuario
            };

            bool result = false;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<bool>(_sqlTran, p);
            }

            return result;
        }

        public class Produto
        {
            public int CodigoProduto { get; set; }
            public string? Descricao { get; set; }
            public string? Referencia { get; set; }
            public string? Localizacao { get; set; }
            public string? Marca { get; set; }
            public string? Categoria { get; set; }
            public decimal ValorUnitario { get; set; }
            public int Quantidade { get; set; }
            public bool Ativo { get; set; }
            public DateTime DataInclusao { get; set; }
            public string? UsuarioIncluiu { get; set; }
            public int CodigoFornecedor { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoProduto { get; set; }
            public string? Descricao { get; set; }
            public string? Referencia { get; set; }
            public string? Localizacao { get; set; }
            public string? Marca { get; set; }
            public string? Categoria { get; set; }
            public decimal ValorUnitario { get; set; }
            public int Quantidade { get; set; }
            public int CodigoFornecedor { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}