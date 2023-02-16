using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CadastroClienteBff.Database.Config
{
    public abstract class BaseConfig<T> : IEntityTypeConfiguration<T> where T : class
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            indices(builder);
            keys(builder);
            fk(builder);
            columns(builder);
        }
        public abstract void indices(EntityTypeBuilder<T> cfg);
        public abstract void keys(EntityTypeBuilder<T> cfg);
        public abstract void fk(EntityTypeBuilder<T> cfg);
        public abstract void columns(EntityTypeBuilder<T> cfg);
    }
}
