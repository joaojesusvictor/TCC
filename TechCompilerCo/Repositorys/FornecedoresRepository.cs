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
    public class FornecedoresRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC Forn_Fornecedores_TRAN @Modo, @CodigoFornecedor, @RazaoSocial, @NomeFantasia, @Documento, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais, @Telefone1, @Email, @UsuarioTran";

        public FornecedoresRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        public async Task<IEnumerable<BaseRepository.Combo>> ComboFornecedoresAsync(bool addBranco = false)
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            var result = await _db.QueryAsync<Fornecedor>(_sqlTran, p);

            var combo = new List<BaseRepository.Combo>();

            foreach (var r in result)
                combo.Add(new BaseRepository.Combo() { Id = r.CodigoFornecedor.ToString(), Nome = r.NomeFantasia });

            if (addBranco)
                combo.Insert(0, new BaseRepository.Combo());

            return combo;
        }

        public async Task<IEnumerable<Fornecedor>> GetFornecedoresAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Fornecedor> results = new List<Fornecedor>();

            using (_db)
            {
                results = await _db.QueryAsync<Fornecedor>(_sqlTran, p);
                _db.Dispose();
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

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<Fornecedor>(_sqlTran, p);
                _db.Dispose();
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

            using (_db)
            {
                result = await _db.QueryFirstOrDefaultAsync<int>(_sqlTran, p);
                _db.Dispose();
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
                CodigoFornecedor = id,
                UsuarioTran = codigoUsuario
            };

            using (_db)
            {
                await _db.ExecuteAsync(_sqlTran, p);
                _db.Dispose();
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