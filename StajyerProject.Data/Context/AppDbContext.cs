using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using StajyerProject.Core.DTO;
using StajyerProject.Core.Entity;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace StajyerProject.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<MesajResponse> Mesajlar { get; set; }
        // Dapper desteği: mevcut bağlantı üzerinden
        public IDbConnection CreateConnection()
            => new SqlConnection(Database.GetDbConnection().ConnectionString);
    }
}
