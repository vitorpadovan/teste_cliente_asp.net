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
            
            var servidor = "localhost";
            var usuario = "root";
            var senha = "123";
            var porta = "3306";
            var banco = "inicial";
            var auxVsar = Environment.GetEnvironmentVariable("SERVIDOR");
            if (auxVsar != null && String.Empty.CompareTo(auxVsar) != 0){
                servidor = auxVsar;
                usuario = Environment.GetEnvironmentVariable("USUARIO");
                senha = Environment.GetEnvironmentVariable("SENHA");
                porta = Environment.GetEnvironmentVariable("PORTA");
                banco = Environment.GetEnvironmentVariable("BANCO");
            }
            var resposta = $"server={servidor}; port={porta}; database={banco}; uid={usuario}; password={senha};Allow User Variables=False";
            return resposta;
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
