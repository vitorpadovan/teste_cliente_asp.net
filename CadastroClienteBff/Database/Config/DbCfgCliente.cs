using CadastroClienteBff.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CadastroClienteBff.Database.Config
{
    public class DbCfgCliente : BaseConfig<Cliente>
    {
        public override void columns(EntityTypeBuilder<Cliente> cfg)
        {
            cfg.Property(p => p.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();
            cfg.Property(p => p.CpfCnpj)
                .HasColumnType("bigint")
                .IsRequired();
        }

        public override void fk(EntityTypeBuilder<Cliente> cfg)
        {

        }

        public override void indices(EntityTypeBuilder<Cliente> cfg)
        {
            cfg.HasIndex(p => p.CpfCnpj).IsUnique();
        }

        public override void keys(EntityTypeBuilder<Cliente> cfg)
        {
            cfg.HasKey(p => p.Id);
        }
    }
}
