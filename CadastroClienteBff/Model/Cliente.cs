using CadastroClienteBff.Model.Enums;

namespace CadastroClienteBff.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CpfCnpj { get; set; }
        public TipoDocumento tipoDocumento { get; set; }

    }
}
