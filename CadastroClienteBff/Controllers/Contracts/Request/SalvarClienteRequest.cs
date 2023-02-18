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
    [StringLength(20, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.", MinimumLength = 11)]
    [Display(Name = "CPF - CNPJ")]
    public string? cpfCnpj { get; set; }

    [Required(ErrorMessage = "{0} não informado")]
    [Range(1,2,  ErrorMessage = "Valor do {0} deve ser entre {1} e {2}.")]
    [Display(Name = "Tipo de Documento")]
    public TipoDocumento? tipoDocumento { get; set; }

    [Display(Name = "Release Date")]
    public DateOnly? dataNascimento { get; set; }
}

