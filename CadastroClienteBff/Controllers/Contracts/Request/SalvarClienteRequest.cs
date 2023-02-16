using CadastroClienteBff.Model.Enums;

namespace CadastroClienteBff.Controllers.Contracts.Request;

public class SalvarClienteRequest
{
    public string nome { get; set; }
    public int cpfCnpj { get; set; }
    public TipoDocumento tipoDocumento { get; set; }
}

