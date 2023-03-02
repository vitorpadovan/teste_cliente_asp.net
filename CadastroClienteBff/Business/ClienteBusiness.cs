using AutoMapper;
using CadastroClienteBff.Config.Exceptions;
using CadastroClienteBff.Database;
using CadastroClienteBff.Model;
using CadastroClienteBff.Config.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using CadastroClienteBff.Config.Const;

namespace CadastroClienteBff.Business
{
    public class ClienteBusiness
    {
        private ContextoBanco cx = new ContextoBanco();
        public void SalvarCliente(Cliente cliente){
            //NormalizarDocumento(cliente);
            cliente.CpfCnpj = cliente.CpfCnpj.ToNormalizeString();
            cliente.cep = cliente.cep.ToNormalizeString();
            var dbCliente = cx.Cliente;
            
            VerificaDataNascimento(cliente);
            ValidarDocumento(cliente);
            VerificarSeJaExiste(cliente);

            dbCliente.Add(cliente);
            cx.SaveChanges();
        }

        private string AplicarMascaraDocumento(Cliente c){
            switch (c.tipoDocumento) {
                case Model.Enums.TipoDocumento.Cpf:
                    return "CPF "+ c.CpfCnpj.MaskNumber(@"000\.000\.000\-00");
                case Model.Enums.TipoDocumento.CNPJ:
                    return "CNPJ " + c.CpfCnpj.MaskNumber(@"00\.000\.000\/0000\-00");
                default:
                    return "CPF/CNPJ "+c.CpfCnpj;
            }
        }
        public List<Cliente> GetListaClientes()
        {
            return cx.Cliente.ToList();
        }

        public bool DeletarCliente(int id)
        {
            Cliente cliente = GetCliente(id);
            //TODO implementar impedimento caso existir relacionamento com algum orçamento e etc no banco
            cx.Cliente.Remove(cliente);
            cx.SaveChanges();
            return true;
        }

        public Cliente AtualizarCliente(int id, Cliente cliente){
            var clienteOriginal = GetCliente(id);
            clienteOriginal.cep = cliente.cep;
            clienteOriginal.tipoDocumento = cliente.tipoDocumento;
            clienteOriginal.dataNascimento = cliente.dataNascimento;
            clienteOriginal.endereco = cliente.endereco;
            clienteOriginal.Nome = cliente.Nome;
            //TODO implementar impedimento de alterar CPF caso existir notas emitidas no nome do cliente

            ValidarDocumento(clienteOriginal);
            VerificaDataNascimento(clienteOriginal);
            if (clienteOriginal.CpfCnpj != cliente.CpfCnpj){
                VerificarCpfDuplicadoEmOutroId(id, cliente);
                clienteOriginal.CpfCnpj = cliente.CpfCnpj;
            }
                

            cx.Cliente.Update(clienteOriginal);
            cx.SaveChanges();
            return cliente;
        }
        public Cliente GetCliente(int id)
        {
            var cliente = cx.Cliente.Where(x => x.Id == id).SingleOrDefault();
            if (cliente == null)
                throw new HttpResponseException(404, new ResponseData() { codError = (int)ErrorCodes.NaoEncontrado, Message = $@"Cliente com id {id} não encontrado" });
            return cliente;
        }
        private static bool VerificaDataNascimento(Cliente c)
        {
            var resposta = c.dataNascimento > DateOnly.FromDateTime(DateTime.Now);
            if (resposta)
                throw new HttpResponseException(400, new ResponseData() { codError = (int)ErrorCodes.DataNascimentoErrada, Message = "Data de nascimento deve ser menor do que a data atual" });
            return resposta;
        }
        private bool VerificarSeJaExiste(Cliente c)
        {
            var resposta = cx.Cliente.Where(p => p.CpfCnpj == c.CpfCnpj).Any();
            if(resposta)
                throw new HttpResponseException(400, new ResponseData() { codError = (int)ErrorCodes.JaCadastrado, Message = $"{AplicarMascaraDocumento(c)} já cadastrado" });
            return resposta;
        }

        private bool VerificarCpfDuplicadoEmOutroId(int id, Cliente c)
        {
            var pesquisarCpf = cx.Cliente.Where(x => x.CpfCnpj == c.CpfCnpj).SingleOrDefault();
            if (pesquisarCpf == null)
                return false; 
            if (pesquisarCpf.Id != id)
                throw new HttpResponseException(400, new ResponseData() { codError = (int)ErrorCodes.CpfDuplicado, Message = $"{AplicarMascaraDocumento(c)} já cadastrado com o nome {pesquisarCpf.Nome} e id {pesquisarCpf.Id}" });
            return false; //Não existe CPF duplicado
        }

        private bool ValidarDocumento(Cliente c){
            var tipo = (Model.Enums.TipoDocumento) c.tipoDocumento;
            var resultado = true;
            switch (tipo)
            {
                case Model.Enums.TipoDocumento.Cpf:
                    resultado =  ValidaCPF(c.CpfCnpj.PadLeft(11,'0'));
                    break;
                case Model.Enums.TipoDocumento.CNPJ:
                    resultado =  ValidaCNPJ(c.CpfCnpj.PadLeft(14,'0'));
                    break;
                default:
                    resultado =  false;
                    break;
            }
            if(!resultado)
                throw new HttpResponseException(400, new ResponseData() { codError = (int)ErrorCodes.DocumentoErrado, Message = $"{AplicarMascaraDocumento(c)} inválido e/ou tipo de documento escolhido errado" });
            return resultado;
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
