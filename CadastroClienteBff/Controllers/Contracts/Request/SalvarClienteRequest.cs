using CadastroClienteBff.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace CadastroClienteBff.Controllers.Contracts.Request;

public class SalvarClienteRequest
{

    [Required(ErrorMessage = "{0} não informado")]
    [Display(Name = "Nome")]
    [StringLength(150, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
    public string? nome { get; set; }

    [Required(ErrorMessage = "{0} não informado")]
    [StringLength(18, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.", MinimumLength = 11)]
    [Display(Name = "CPF - CNPJ")]
    public string? cpfCnpj { get; set; }

    [Required(ErrorMessage = "{0} não informado")]
    [Display(Name = "Tipo de Documento")]
    public TipoDocumento tipoDocumento { get; set; }

    [Display(Name = "Release Date")]
    public DateOnly? dataNascimento { get; set; }
    
    [Display(Name = "CEP")]
    [Required(ErrorMessage = "{0} não informado")]
    [StringLength(9, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.", MinimumLength = 8)]
    public string? cep { get; set; }

    [Display(Name = "Endereço")]
    [Required(ErrorMessage = "{0} não informado")]
    [StringLength(150, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.", MinimumLength = 4)]
    public string? endereco { get; set; }

    [Display(Name = "Número")]
    [Required(ErrorMessage = "{0} não informado")]
    [Range(minimum:1, maximum:Double.MaxValue,ErrorMessage = "{0} deve ter entre {2} e {1}.")]
    public int? numero { get; set; }
}