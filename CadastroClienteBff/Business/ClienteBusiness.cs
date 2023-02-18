﻿using CadastroClienteBff.Config.Exceptions;
using CadastroClienteBff.Database;
using CadastroClienteBff.Model;
using System.Data.SqlTypes;
using System.Drawing;

namespace CadastroClienteBff.Business
{
    public class ClienteBusiness
    {
        private ContextoBanco cx = new ContextoBanco();
        public void salvarCliente(Cliente cliente)
        {
            NormalizarDocumento(cliente);
            var dbCliente = cx.Cliente;
            if (verificaDataNascimento(cliente))
                throw new HttpResponseException(400, new ResponseData() { codError = 3, Message = "Data de nascimento deve ser menor do que a data atual" });
            if (!documentoValido(cliente))
                throw new HttpResponseException(400, new ResponseData() { codError = 1, Message = $"{AplicarMascaraDocumento(cliente)} inválido e/ou tipo de documento escolhido errado" });
            if (verificarSeJaExiste(cliente))
                throw new HttpResponseException(400, new ResponseData() { codError = 2, Message = $"{AplicarMascaraDocumento(cliente)} já cadastrado" });
            dbCliente.Add(cliente);
            cx.SaveChanges();
        }

        private string AplicarMascaraDocumento(Cliente c)
        {
            switch (c.tipoDocumento) {
                case Model.Enums.TipoDocumento.Cpf:
                    return "CPF "+Int64.Parse(c.CpfCnpj).ToString(@"000\.000\.000\-00");
                case Model.Enums.TipoDocumento.CNPJ:
                    return "CNPJ " + Int64.Parse(c.CpfCnpj).ToString(@"00\.000\.000\/0000\-00");
                default:
                    return "CPF/CNPJ "+c.CpfCnpj;
            }
        }

        private static void NormalizarDocumento(Cliente cliente)
        {
            cliente.CpfCnpj = cliente.CpfCnpj.Replace(".", "");
            cliente.CpfCnpj = cliente.CpfCnpj.Replace("_", "");
            cliente.CpfCnpj = cliente.CpfCnpj.Replace("-", "");
            cliente.CpfCnpj = cliente.CpfCnpj.Replace("/", "");
            cliente.CpfCnpj = cliente.CpfCnpj.Replace("\\", "");
        }

        public List<Cliente> getListaClientes()
        {
            return cx.Cliente.ToList();
        }
        
        private bool verificaDataNascimento(Cliente c)
        {
            return c.dataNascimento > DateOnly.FromDateTime(DateTime.Now);
        }
        private bool verificarSeJaExiste(Cliente c)
        {
            return cx.Cliente.Where(p=>p.CpfCnpj == c.CpfCnpj).Any();
        }

        private bool documentoValido(Cliente c){
            var tipo = (Model.Enums.TipoDocumento) c.tipoDocumento;
            switch (tipo)
            {
                case Model.Enums.TipoDocumento.Cpf:
                    return ValidaCPF(c.CpfCnpj.PadLeft(11,'0'));
                case Model.Enums.TipoDocumento.CNPJ:
                    return ValidaCNPJ(c.CpfCnpj.PadLeft(14,'0'));
                default:
                    return false;
            }
        }

        public static bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");
            if (valor.Length != 11)
                return false;
            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;
            if (igual || valor == "12345678909")
                return false;
            int[] numeros = new int[11];
            try
            {
                for (int i = 0; i < 11; i++)
                    numeros[i] = int.Parse(
                      valor[i].ToString());
            }catch(FormatException ex)
            {
                return false;
            }
            
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];
            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];
            resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;
            return true;
        }

        public static bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;
            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }
                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (
                         resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }
    }


}
