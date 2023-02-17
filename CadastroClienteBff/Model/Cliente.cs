using CadastroClienteBff.Model.Enums;

namespace CadastroClienteBff.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public TipoDocumento tipoDocumento { get; set; }
        public DateOnly dataNascimento { get; set; }

    }
}
