using CadastroClienteBff.Model.Enums;

namespace CadastroClienteBff.Controllers.Contracts.Request;

public class SalvarClienteRequest
{
    public string nome { get; set; }
    public string cpfCnpj { get; set; }
    public TipoDocumento tipoDocumento { get; set; }
    public DateOnly dataNascimento { get; set; }
}

