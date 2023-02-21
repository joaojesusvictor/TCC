﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class FuncionariosRepository : BaseRepository
    {
        private string _sqlTran = "EXEC Func_Funcionarios_TRAN @Modo, @CodigoFuncionario, @NomeFuncionario, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais, @DataNascimento, @Cpf, @Sexo, @Telefone1, @Cargo, @UsuarioTran";

        public FuncionariosRepository()
        {

        }

        //public async Task<IEnumerable<Funcionario>> GetFuncionariosAsync()
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 5
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.QueryAsync<Funcionario>(_sqlTran, p);
        //    });
        //}

        //public async Task<Funcionario> GetFuncionarioAsync(int id)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 4,
        //        CodigoFuncionario = id
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.QueryFirstOrDefaultAsync<Funcionario>(_sqlTran, p);
        //    });
        //}

        //public async Task<int> CreateAsync(FuncionariosViewModel model)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 1,
        //        CodigoFuncionario = model.CodigoFuncionario,
        //        UsuarioTran = _currentUser.IdUsuario,
        //        NomeFuncionario = model.NomeFuncionario,
        //        DataNascimento = model.DataNascimento,
        //        Telefone1 = model.Telefone1,
        //        Cpf = model.Cpf,
        //        Cep = model.Cep,
        //        Endereco = model.Endereco,
        //        Numero = model.Numero,
        //        Complemento = model.Complemento,
        //        Bairro = model.Bairro,
        //        Cidade = model.Cidade,
        //        Uf = model.Uf,
        //        Pais = model.Pais,
        //        Sexo = model.Sexo,
        //        Cargo = model.Cargo
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.GetFirstAsync<int>(_sqlTran, p);
        //    });
        //}

        //public async Task<int> UpdateAsync(FuncionariosViewModel model)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 2,
        //        CodigoFuncionario = model.CodigoFuncionario,
        //        UsuarioTran = _currentUser.IdUsuario,
        //        NomeFuncionario = model.NomeFuncionario,
        //        DataNascimento = model.DataNascimento,
        //        Telefone1 = model.Telefone1,
        //        Cpf = model.Cpf,
        //        Cep = model.Cep,
        //        Endereco = model.Endereco,
        //        Numero = model.Numero,
        //        Complemento = model.Complemento,
        //        Bairro = model.Bairro,
        //        Cidade = model.Cidade,
        //        Uf = model.Uf,
        //        Pais = model.Pais,
        //        Sexo = model.Sexo,
        //        Cargo = model.Cargo
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.ExecuteAsync<int>(_sqlTran, p);
        //    });
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 3,
        //        CodigoFuncionario = id
        //    };

        //    await UsarSql(async conn =>
        //    {
        //        return await conn.ExecuteAsync(_sqlTran, p);
        //    });
        //}

        public class Funcionario                        //Reclama se a prop nao é anulavel
        {
            public int CodigoFuncionario { get; set; }
            public DateTime DataInclusao { get; set; }
            public DateTime DataUltimaAlteracao { get; set; }
            public string UsuarioIncluiu { get; set; }
            public string UsuarioUltimaAlteracao { get; set; }
            public string NomeFuncionario { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Telefone1 { get; set; }
            public string Cpf { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public int Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Uf { get; set; }
            public string Pais { get; set; }
            public string Sexo { get; set; }
            public string Email { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoFuncionario { get; set; }
            public string NomeFuncionario { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Telefone1 { get; set; }
            public string Cpf { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public int Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Uf { get; set; }
            public string Pais { get; set; }
            public string Sexo { get; set; }
            public string Cargo { get; set; }
            public int UsuarioTran { get; set; }
        }
    }
}