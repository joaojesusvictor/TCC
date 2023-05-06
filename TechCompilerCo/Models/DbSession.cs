using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Models
{
    public class DbSession
    {
        public IDbConnection SqlConnection()
        {
            return new SqlConnection("Server=DESKTOP-IRV7A0M;Initial Catalog=TccProducao;Integrated Security=True;TrustServerCertificate=true;");
        }
    }
}