using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Models
{
    public class DbSession /*: IDisposable*/
    {
        //public IDbConnection Connection { get; }

        //public DbSession(IConfiguration configuration)
        //{
        //    Connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        //    Connection.Open();
        //}

        ////public void Dipose() => Connection?.Dispose();

        //public void Dispose()
        //{
        //    Connection?.Dispose();
        //}

        public IDbConnection SqlConnection()
        {
            return new SqlConnection("Server=DESKTOP-HMIHBNE;Initial Catalog=TccProducao;Integrated Security=True;TrustServerCertificate=true;");
        }
    }
}