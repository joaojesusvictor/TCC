using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class ClientesRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Clien_Clientes_TRAN @Modo, @CodigoCliente, @NomeCliente, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais, @DataNascimento, @Cpf, @Sexo, @Email, @Telefone1, @UsuarioTran";

        public ClientesRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Cliente> results = new List<Cliente>();

            using (_db)
            {
                results = await _db.QueryAsync<Cliente>(_sqlTran, p);
                _db.Dispose();
            }

            return results;
        }

        public async Task<Cliente> GetClienteAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoCliente = id
            };

            Cliente result = new();

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<Cliente>(_sqlTran, p);
                _db.Dispose();
            }

            return result;
        }

        public async Task<int> CreateAsync(ClientesViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoCliente = model.CodigoCliente,
                UsuarioTran = model.CodigoUsuario,
                NomeCliente = model.NomeCliente,
                DataNascimento = model.DataNascimento,
                Email = model.Email,
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
                Sexo = model.Sexo
            };

            int result = 0;

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
                _db.Dispose();
            }

            return result;
        }

        public async Task<int> UpdateAsync(ClientesViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoCliente = model.CodigoCliente,
                UsuarioTran = model.CodigoUsuario,
                NomeCliente = model.NomeCliente,
                DataNascimento = model.DataNascimento,
                Email = model.Email,
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
                Sexo = model.Sexo
            };

            int result = 0;

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
                _db.Dispose();
            }

            return result;
        }

        public async Task DeleteAsync(int id, int codigoUsuario)
        {
            var p = new ParametrosTran()
            {
                Modo = 3,
                CodigoCliente = id,
                UsuarioTran = codigoUsuario
            };

            using (_db)
            {
                await _db.ExecuteAsync(_sqlTran, p);
                _db.Dispose();
            }
        }

        public class Cliente
        {
            public int CodigoCliente { get; set; }
            public DateTime? DataInclusao { get; set; }
            public DateTime? DataUltimaAlteracao { get; set; }
            public string? UsuarioIncluiu { get; set; }
            public string? UsuarioUltimaAlteracao { get; set; }
            public string? NomeCliente { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string? Email { get; set; }
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
        }

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoCliente { get; set; }
            public string? NomeCliente { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string? Email { get; set; }
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
            public int UsuarioTran { get; set; }
        }
    }
}