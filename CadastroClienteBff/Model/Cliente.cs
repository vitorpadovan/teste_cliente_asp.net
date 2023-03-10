using CadastroClienteBff.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CadastroClienteBff.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public TipoDocumento tipoDocumento { get; set; }
        public DateOnly dataNascimento { get; set; }
        public string cep { get; set; }
        public string endereco { get; set; }
        public int numero { get; set; }

    }
}
