using Consultorio_Seguros.Models.Application;
using Microsoft.EntityFrameworkCore;

namespace Consultorio_Seguros.Models.BD
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Asegurados> Asegurados { get; set; }
        public DbSet<Seguros> Seguros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configurationCaptada = ApplicationMethods.GetFieldJson("configuration", "DataBase");
                var Server = configurationCaptada["server"];
                var Database = configurationCaptada["databasename"];
                var id = configurationCaptada["user"];
                var password = configurationCaptada["password"];
                var TrustServerCertificate = configurationCaptada["trustServerCertificate"];
                var PersistSecurityInfo = configurationCaptada["persistSecurityInfo"].ToString();
                var finalChain = $"Server={Server};Database={Database};user id={id};password={password};TrustServerCertificate={TrustServerCertificate};Persist Security Info={PersistSecurityInfo}";
                optionsBuilder.UseSqlServer(finalChain);
            }
        }
    }
}
