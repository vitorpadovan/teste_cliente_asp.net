using AutoMapper;
using CadastroClienteBff.Business;
using CadastroClienteBff.Config.Exceptions;
using CadastroClienteBff.Controllers.Contracts.Request;
using CadastroClienteBff.Controllers.Contracts.Response;
using CadastroClienteBff.Model;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteBff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        ClienteBusiness c = new ClienteBusiness();
        private IMapper mapper;

        public ClienteController()
        {
            this.mapper = ConfigurarMapper();
        }

        [HttpPost(Name = "SalvarCliente")]
        public SalvarClienteResponse salvarCliente(SalvarClienteRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(1, "Chato");

            var cliente = mapper.Map<Cliente>(request);
            this.c.salvarCliente(cliente);
            var response = mapper.Map<SalvarClienteResponse>(cliente);
            return response;
        }

        [HttpGet(Name = "GetClientes")]
        public List<SalvarClienteResponse> getListaCliente()
        {
            List<SalvarClienteResponse> response = new List<SalvarClienteResponse>();
            c.getListaClientes().ForEach((x) => response.Add(mapper.Map<SalvarClienteResponse>(x)));
            return response;
        }

        private static IMapper ConfigurarMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, SalvarClienteRequest>();
                cfg.CreateMap<SalvarClienteRequest, Cliente>();
                cfg.CreateMap<SalvarClienteResponse, Cliente>();
                cfg.CreateMap<Cliente, SalvarClienteResponse>();
            });

            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
