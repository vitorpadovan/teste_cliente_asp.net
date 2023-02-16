using AutoMapper;
using CadastroClienteBff.Business;
using CadastroClienteBff.Controllers.Contracts.Request;
using CadastroClienteBff.Model;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteBff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController
    {
        ClienteBusiness c = new ClienteBusiness();

        
        [HttpPost(Name = "GetSss")]
        public Cliente salvarCliente(SalvarClienteRequest request)
        {
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
    }
}
