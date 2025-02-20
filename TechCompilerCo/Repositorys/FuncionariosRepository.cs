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
    public class FuncionariosRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Func_Funcionarios_TRAN @Modo, @CodigoFuncionario, @NomeFuncionario, @DataContratacao, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais, @DataNascimento, @Cpf, @Sexo, @Telefone1, @Cargo, @UsuarioTran";

        public FuncionariosRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<SelectListItem>> ComboFuncionariosAsync(bool addBranco = false)
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            var result = await _db.QueryAsync<Funcionario>(_sqlTran, p);

            var combo = new List<SelectListItem>();

            foreach (var r in result)
                combo.Add(new SelectListItem() { Value = r.CodigoFuncionario.ToString(), Text = r.NomeFuncionario });

            if (addBranco)
                combo.Insert(0, new SelectListItem());

            return combo;
        }

        public async Task<IEnumerable<Funcionario>> GetFuncionariosAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Funcionario> results = new List<Funcionario>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Funcionario>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Funcionario> GetFuncionarioAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoFuncionario = id
            };

            Funcionario result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Funcionario>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> CreateAsync(FuncionariosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoFuncionario = model.CodigoFuncionario,
                UsuarioTran = model.CodigoUsuario,
                NomeFuncionario = model.NomeFuncionario,
                DataContratacao = model.DataContratacao,
                DataNascimento = model.DataNascimento,
                Telefone1 = model.Telefone1,
                Cpf = model.Cpf,
                Cep = model.Cep,
                Endereco = model.Endereco,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Uf = model.Uf,
                Pais = model.Pais,
                Sexo = model.Sexo,
                Cargo = model.Cargo
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAsync(FuncionariosViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoFuncionario = model.CodigoFuncionario,
                UsuarioTran = model.CodigoUsuario,
                NomeFuncionario = model.NomeFuncionario,
                DataContratacao = model.DataContratacao,
                DataNascimento = model.DataNascimento,
                Telefone1 = model.Telefone1,
                Cpf = model.Cpf,
                Cep = model.Cep,
                Endereco = model.Endereco,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Uf = model.Uf,
                Pais = model.Pais,
                Sexo = model.Sexo,
                Cargo = model.Cargo
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
                CodigoFuncionario = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        public class Funcionario
        {
            public int CodigoFuncionario { get; set; }
            public DateTime? DataContratacao { get; set; }
            public DateTime? DataInclusao { get; set; }
            public DateTime? DataUltimaAlteracao { get; set; }
            public string? UsuarioIncluiu { get; set; }
            public string? UsuarioUltimaAlteracao { get; set; }
            public string? NomeFuncionario { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string? Telefone1 { get; set; }
            public string? Cpf { get; set; }
            public string? Cep { get; set; }
            public string? Endereco { get; set; }
            public int Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Uf { get; set; }
            public string? Pais { get; set; }
            public string? Sexo { get; set; }
            public string? Email { get; set; }
            public string? Cargo { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoFuncionario { get; set; }
            public string? NomeFuncionario { get; set; }
            public DateTime? DataContratacao { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string? Telefone1 { get; set; }
            public string? Cpf { get; set; }
            public string? Cep { get; set; }
            public string? Endereco { get; set; }
            public int Numero { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Uf { get; set; }
            public string? Pais { get; set; }
            public string? Sexo { get; set; }
            public string? Cargo { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}