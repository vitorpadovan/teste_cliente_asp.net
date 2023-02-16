using CadastroClienteBff.Database.Config;
using CadastroClienteBff.Model;
using Microsoft.EntityFrameworkCore;

namespace CadastroClienteBff.Database
{
    public class ContextoBanco : DbContext
    {
        public DbSet<Cliente> Cliente { get; set; }
        public static String dadosDeAcesso()
        {
            //String servidor = Environment.GetEnvironmentVariable("SERVIDOR");
            //String usuario = Environment.GetEnvironmentVariable("USUARIO");
            //String senha = Environment.GetEnvironmentVariable("SENHA");
            //String porta = Environment.GetEnvironmentVariable("PORTA");
            //String banco = Environment.GetEnvironmentVariable("DATABASE");

            String servidor = "localhost";
            String usuario = "root";
            String senha = "123";
            String porta = "3306";
            String banco = "inicial";
            return $"server={servidor}; port={porta}; database={banco}; uid={usuario}; password={senha};Allow User Variables=False";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(dadosDeAcesso(), ServerVersion.AutoDetect(dadosDeAcesso()));
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbCfgCliente());
        }
    }
}
