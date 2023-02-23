using Microsoft.Data.SqlClient;
using System.Data;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Models
{
    public class DbSession
    {
        public IDbConnection Connection { get; set; }

        public DbSession(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            Connection.Open();
        }

        public void Dipose() => Connection?.Dispose();
    }
}