using CadastroClienteBff.Database;
using CadastroClienteBff.Model;

namespace CadastroClienteBff.Business
{
    public class ClienteBusiness
    {
        private ContextoBanco cx = new ContextoBanco();
        public void salvarCliente(Cliente cliente)
        {
            var teste = cx.Cliente;
            teste.Add(cliente);
            cx.SaveChanges();
        }
    }
}
