using System;
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
        private string _sqlTran = "";

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
        //        IdFuncionario = id
        //    };

        //    return await UsarSql(async conn =>
        //    {
        //        return await conn.QueryFirstOrDefaultAsync<Funcionario>(_sqlTran, p);
        //    });
        //}

        //public async Task CreateAsync(FuncionariosViewModel model)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 1,
        //        IdFuncionario = model.IdFuncionario,
        //        UsuarioAdm = model.UsuarioAdm,
        //        DataCadastro = null,
        //        DataUltimaAlteracao = null,
        //        UsuarioCadastrou = _currentUser.IdUsuario,
        //        UsuarioUltimaAlteracao = null,
        //        Nome = model.Nome,
        //        Email = model.Email,
        //        DataNascimento = model.DataNascimento,
        //        Telefone = model.Telefone,
        //        Cpf = model.Cpf,
        //        Cep = model.Cep,
        //        Endereco = model.Endereco,
        //        Numero = model.Numero,
        //        Complemento = model.Complemento,
        //        Bairro = model.Bairro,
        //        Cidade = model.Cidade,
        //        Estado = model.Estado,
        //        Pais = model.Pais
        //    };

        //    await UsarSql(async conn =>
        //    {
        //        return await conn.ExecuteAsync(_sqlTran, p);
        //    });
        //}

        //public async Task UpdateAsync(FuncionariosViewModel model)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 2,
        //        IdFuncionario = model.IdFuncionario,
        //        UsuarioAdm = model.UsuarioAdm,
        //        DataCadastro = model.DataCadastro,
        //        //DataUltimaAlteracao = DateTime.Now,
        //        UsuarioCadastrou = model.UsuarioCadastrou,
        //        UsuarioUltimaAlteracao = _currentUser.IdUsuario,
        //        Nome = model.Nome,
        //        Email = model.Email,
        //        DataNascimento = model.DataNascimento,
        //        Telefone = model.Telefone,
        //        Cpf = model.Cpf,
        //        Cep = model.Cep,
        //        Endereco = model.Endereco,
        //        Numero = model.Numero,
        //        Complemento = model.Complemento,
        //        Bairro = model.Bairro,
        //        Cidade = model.Cidade,
        //        Estado = model.Estado,
        //        Pais = model.Pais
        //    };

        //    await UsarSql(async conn =>
        //    {
        //        return await conn.ExecuteAsync(_sqlTran, p);
        //    });
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var p = new ParametrosTran()
        //    {
        //        Modo = 3,
        //        IdFuncionario = id
        //    };

        //    await UsarSql(async conn =>
        //    {
        //        return await conn.ExecuteAsync(_sqlTran, p);
        //    });
        //}

        public class Funcionario                        //Reclama se a prop nao é anulavel
        {
            public int IdFuncionario { get; set; }
            public DateTime DataCadastro { get; set; }
            public DateTime DataUltimaAlteracao { get; set; }
            public string UsuarioCadastrou { get; set; }
            public string UsuarioUltimaAlteracao { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Telefone { get; set; }
            public string Cpf { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public int Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Pais { get; set; }
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public bool UsuarioAdm { get; set; }
            public int IdFuncionario { get; set; }
            public DateTime? DataCadastro { get; set; }
            public DateTime? DataUltimaAlteracao { get; set; }
            public string UsuarioCadastrou { get; set; }
            public string UsuarioUltimaAlteracao { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Telefone { get; set; }
            public string Cpf { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public int Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Pais { get; set; }
        }
    }
}