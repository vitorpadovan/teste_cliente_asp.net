using AutoMapper;
using CadastroClienteBff.Business;
using CadastroClienteBff.Config.Exceptions;
using CadastroClienteBff.Controllers.Contracts.Request;
using CadastroClienteBff.Model;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteBff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        ClienteBusiness c = new ClienteBusiness();

        
        [HttpPost(Name = "SalvarCliente")]
        public Cliente salvarCliente(SalvarClienteRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(1, "Chato");
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, SalvarClienteRequest>();
                cfg.CreateMap<SalvarClienteRequest, Cliente>();
            });

            var mapper = configuration.CreateMapper();

            var c = mapper.Map<Cliente>(request);
            this.c.salvarCliente(c);
            return c;
        }

        [HttpGet(Name = "GetClientes")]

        public List<Cliente> getListaCliente()
        {
            return c.getListaClientes();
        }
    }
}
