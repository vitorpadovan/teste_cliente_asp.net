using CadastroClienteBff.Config.Extensions;
using CadastroClienteBff.Model.Enums;
using Microsoft.Extensions.WebEncoders.Testing;

namespace CadastroClienteBff.Controllers.Contracts.Response;
public class SalvarClienteResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }

    private string _CpfCnpj;
    public string CpfCnpj { get {
            switch (this.tipoDocumento){
                case TipoDocumento.Cpf:
                    return _CpfCnpj.MaskNumber(@"000\.000\.000\-00");
                case TipoDocumento.CNPJ:
                    return _CpfCnpj.MaskNumber(@"00\.000\.000\/0000\-00");
            }
            return _CpfCnpj; 
        } set { _CpfCnpj = value; } }
    public TipoDocumento tipoDocumento { get; set; }
    public DateOnly dataNascimento { get; set; }

    private string _cep;
    public string cep { get { return _cep.MaskNumber(@"00000\-000"); } set { _cep = value; } }
    public string endereco { get; set; }
    public int numero { get; set; }
}
