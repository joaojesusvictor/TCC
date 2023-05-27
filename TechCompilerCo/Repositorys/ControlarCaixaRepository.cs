using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Abstractions;
using TechCompilerCo.Models;

namespace TechCompilerCo.Repositorys
{
    public class ControlarCaixaRepository
    {
        private readonly IDbConnection _db;
        private string _sqlTran = "EXEC CAX_ControlarCaixa_TRAN @Modo, @CodigoCaixa, @CodigoCliente, @Descricao, @ValorTotal, @ValorDesconto, @ValorEntrada, @ValorSaida, @DataMovimento, @FormaMovimento, @UsuarioTran";
        private string _sqlAbreFechaTran = "EXEC CAX_AbreFechaCaixa_TRAN @Modo, @CodigoAFCaixa, @DataCaixa, @ValorAbertura, @ValorSaldo, @ValorFechamento, @UsuarioTran";

        public ControlarCaixaRepository()
        {
            _db = new DbSession().SqlConnection();
        }

        #region Caixa

        public async Task<IEnumerable<Caixa>> GetCaixasEntradaAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 5
            };

            IEnumerable<Caixa> results = new List<Caixa>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Caixa>(_sqlTran, p);
            }

            return results;
        }

        public async Task<IEnumerable<Caixa>> GetCaixasSaidaAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 6
            };

            IEnumerable<Caixa> results = new List<Caixa>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Caixa>(_sqlTran, p);
            }

            return results;
        }

        public async Task<Caixa> GetCaixaAsync(int id)
        {
            var p = new ParametrosTran()
            {
                Modo = 4,
                CodigoCaixa = id
            };

            Caixa result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<Caixa>(_sqlTran, p);
            }

            return result;
        }
        
        public async Task<int> CreateAsync(ControlarCaixaViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 1,
                CodigoCliente = model.CodigoCliente,
                Descricao = model.Descricao,
                ValorTotal = model.ValorTotal,
                ValorDesconto = model.ValorDesconto,
                ValorEntrada = model.ValorEntrada,
                ValorSaida = model.ValorSaida,
                DataMovimento = model.DataMovimento,
                FormaMovimento = model.FormaMovimento,
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

        public async Task UpdateAsync(ControlarCaixaViewModel model)
        {
            var p = new ParametrosTran()
            {
                Modo = 2,
                CodigoCaixa = model.CodigoCaixa,
                CodigoCliente = model.CodigoCliente,
                Descricao = model.Descricao,
                ValorTotal = model.ValorTotal,
                ValorDesconto = model.ValorDesconto,
                ValorEntrada = model.ValorEntrada,
                ValorSaida = model.ValorSaida,
                DataMovimento = model.DataMovimento,
                FormaMovimento = model.FormaMovimento,
                UsuarioTran = model.CodigoUsuario
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
                CodigoCaixa = id,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlTran, p);
            }
        }

        #endregion

        #region AbreFechaCaixa

        public async Task<IEnumerable<AbreFechaCaixa>> GetAFCaixasAsync()
        {
            var p = new ParametrosAFTran()
            {
                Modo = 5
            };

            IEnumerable<AbreFechaCaixa> results = new List<AbreFechaCaixa>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<AbreFechaCaixa>(_sqlAbreFechaTran, p);
            }

            return results;
        }

        public async Task<AbreFechaCaixa> GetAFCaixaAsync(int id)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 4,
                CodigoAFCaixa = id
            };

            AbreFechaCaixa result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<AbreFechaCaixa>(_sqlAbreFechaTran, p);
            }

            return result;
        }

        public async Task<int> CreateAFCaixaAsync(ControlarCaixaViewModel model)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 1,
                DataCaixa = model.DataCaixa,
                ValorAbertura = model.ValorAbertura,
                ValorFechamento = model.ValorFechamento,
                UsuarioTran = model.CodigoUsuario
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlAbreFechaTran, p);
            }

            return result;
        }

        public async Task<int> UpdateAFCaixaAsync(ControlarCaixaViewModel model)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 2,
                CodigoAFCaixa = model.CodigoAFCaixa,
                DataCaixa = model.DataCaixa,
                ValorAbertura = model.ValorAbertura,
                ValorFechamento = model.ValorFechamento,
                UsuarioTran = model.CodigoUsuario
            };

            int result = 0;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<int>(_sqlAbreFechaTran, p);
            }

            return result;
        }

        public async Task<bool> DeleteAFCaixaAsync(int id, int codigoUsuario, DateTime dataCaixa)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 3,
                CodigoAFCaixa = id,
                DataCaixa = dataCaixa,
                UsuarioTran = codigoUsuario
            };

            bool result = false;

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<bool>(_sqlAbreFechaTran, p);
            }

            return result;
        }

        public async Task<AbreFechaCaixa> GetSaldoAFCaixaAsync(DateTime? data)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 6,
                DataCaixa = data ?? DateTime.Today,
            };

            AbreFechaCaixa result = new();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                result = await conn.QueryFirstOrDefaultAsync<AbreFechaCaixa>(_sqlAbreFechaTran, p);
            }

            return result;
        }

        public async Task UpdateSaldoAFCaixaAsync(DateTime? data, decimal? saldo, int codigoUsuario)
        {
            var p = new ParametrosAFTran()
            {
                Modo = 7,
                DataCaixa = data,
                ValorSaldo = saldo,
                UsuarioTran = codigoUsuario
            };

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(_sqlAbreFechaTran, p);
            }
        }

        #endregion

        #region Relatorio

        public async Task<IEnumerable<Relatorio>> GetRelatorioCaixaAsync()
        {
            var p = new ParametrosTran()
            {
                Modo = 7
            };

            IEnumerable<Relatorio> results = new List<Relatorio>();

            using (var conn = new SqlConnection(_db.ConnectionString))
            {
                conn.Open();

                results = await conn.QueryAsync<Relatorio>(_sqlTran, p);
            }

            return results;
        }

        #endregion

        #region Modais

        public class Caixa
        {
            public int CodigoCaixa { get; set; }
            public int CodigoCliente { get; set; }
            public string? Descricao { get; set; }
            public decimal ValorTotal { get; set; }
            public decimal ValorDesconto { get; set; }
            public decimal ValorEntrada { get; set; }
            public decimal ValorSaida { get; set; }
            public DateTime DataMovimento { get; set; }
            public string? FormaMovimento { get; set; }
            public DateTime DataInclusao { get; set; }
            public string? NomeCliente { get; set; }
        }
        
        public class AbreFechaCaixa
        {
            public int CodigoAFCaixa { get; set; }
            public DateTime DataCaixa { get; set; }
            public decimal ValorAbertura { get; set; }
            public decimal ValorSaldo { get; set; }
            public decimal? ValorFechamento { get; set; }
            public DateTime DataInclusao { get; set; }
        }

        public class Relatorio
        {
            public DateTime DataCaixa { get; set; }
            public decimal ValorAbertura { get; set; }
            public decimal TotalEntrada { get; set; }
            public decimal TotalSaida { get; set; }
            public decimal ValorFechamento { get; set; }
        }

        #endregion

        #region Parametros

        private class ParametrosTran
        {
            public int Modo { get; set; }
            public int CodigoCaixa { get; set; }
            public int CodigoCliente { get; set; }
            public string? Descricao { get; set; }
            public decimal ValorTotal { get; set; }
            public decimal ValorDesconto { get; set; }
            public decimal? ValorEntrada { get; set; }
            public decimal? ValorSaida { get; set; }
            public DateTime? DataMovimento { get; set; }
            public string? FormaMovimento { get; set; }
            public int UsuarioTran { get; set; }
        }

        private class ParametrosAFTran
        {
            public int Modo { get; set; }
            public int CodigoAFCaixa { get; set; }
            public DateTime? DataCaixa { get; set; }
            public decimal ValorAbertura { get; set; }
            public decimal? ValorSaldo { get; set; }
            public decimal? ValorFechamento { get; set; }
            public int UsuarioTran { get; set; }
        }

        #endregion
    }
}