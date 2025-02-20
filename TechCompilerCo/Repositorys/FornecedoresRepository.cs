﻿using System;
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
    public class FornecedoresRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Forn_Fornecedores_TRAN @Modo, @CodigoFornecedor, @RazaoSocial, @NomeFantasia, @Documento, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais, @Telefone1, @Email, @UsuarioTran";

        public FornecedoresRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<SelectListItem>> ComboFornecedoresAsync(bool addBranco = false)
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            var result = await _db.QueryAsync<Fornecedor>(_sqlTran, p);

            var combo = new List<SelectListItem>();

            foreach (var r in result)
                combo.Add(new SelectListItem() { Value = r.CodigoFornecedor.ToString(), Text = r.NomeFantasia });

            if (addBranco)
                combo.Insert(0, new SelectListItem());

            return combo;
        }

        public async Task<IEnumerable<Fornecedor>> GetFornecedoresAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Fornecedor> results = new List<Fornecedor>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Fornecedor>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Fornecedor> GetFornecedorAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoFornecedor = id
            };

            Fornecedor result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Fornecedor>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(FornecedoresViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoFornecedor = model.CodigoFornecedor,
                UsuarioTran = model.CodigoUsuario,
                RazaoSocial = model.RazaoSocial,
                NomeFantasia = model.NomeFantasia,
                Telefone1 = model.Telefone1,
                Documento = model.Documento,
                Cep = model.Cep,
                Endereco = model.Endereco,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Uf = model.Uf,
                Pais = model.Pais,
                Email = model.Email
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(FornecedoresViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoFornecedor = model.CodigoFornecedor,
                UsuarioTran = model.CodigoUsuario,
                RazaoSocial = model.RazaoSocial,
                NomeFantasia = model.NomeFantasia,
                Telefone1 = model.Telefone1,
                Documento = model.Documento,
                Cep = model.Cep,
                Endereco = model.Endereco,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Uf = model.Uf,
                Pais = model.Pais,
                Email = model.Email
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
                CodigoFornecedor = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class Fornecedor
        {
            public int CodigoFornecedor { get; set; }
            public DateTime? DataInclusao { get; set; }
            public DateTime? DataUltimaAlteracao { get; set; }
            public string? UsuarioIncluiu { get; set; }
            public string? UsuarioUltimaAlteracao { get; set; }
            public string? RazaoSocial { get; set; }
            public string? NomeFantasia { get; set; }
            public string? Telefone1 { get; set; }
            public string? Documento { get; set; }
            public string? Cep { get; set; }
            public string? Endereco { get; set; }
            public int Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Uf { get; set; }
            public string? Pais { get; set; }
            public string? Email { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoFornecedor { get; set; }
            public string? RazaoSocial { get; set; }
            public string? NomeFantasia { get; set; }
            public string? Telefone1 { get; set; }
            public string? Documento { get; set; }
            public string? Cep { get; set; }
            public string? Endereco { get; set; }
            public int Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Uf { get; set; }
            public string? Pais { get; set; }
            public string? Email { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}