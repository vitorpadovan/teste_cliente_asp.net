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
        readonly ClienteBusiness _clienteBusiness = new ClienteBusiness();
        private IMapper mapper;

        public ClienteController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "Pesquisar Cliente")]
        public SalvarClienteResponse GetCliente([FromRoute] int id)
        {
            return mapper.Map<SalvarClienteResponse>(_clienteBusiness.GetCliente(id));
        }

        [HttpDelete("{id}", Name = "Deletar Cliente")]
        public bool DeletarCliente([FromRoute] int id)
        {
            return _clienteBusiness.DeletarCliente(id);
        }

        [HttpPost(Name = "Salvar Cliente")]
        public SalvarClienteResponse SalvarCliente(SalvarClienteRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(1, "Dados inválidos");
            var cliente = mapper.Map<Cliente>(request);
            this._clienteBusiness.SalvarCliente(cliente);
            var response = mapper.Map<SalvarClienteResponse>(cliente);
            return response;
        }

        [HttpPut("{id}", Name = "Atualizar Cliente")]
        public SalvarClienteResponse AtualizarCliente([FromBody] SalvarClienteRequest request, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(1, "Dados inválidos");
            var cliente = mapper.Map<Cliente>(request);
            var resposta = this._clienteBusiness.AtualizarCliente(id, cliente);
            return mapper.Map<SalvarClienteResponse>(resposta);

        }

        [HttpGet(Name = "GetClientes")]
        public List<SalvarClienteResponse> GetListaCliente()
        {
            List<SalvarClienteResponse> response = new List<SalvarClienteResponse>();
            _clienteBusiness.GetListaClientes().ForEach((x) => response.Add(mapper.Map<SalvarClienteResponse>(x)));
            return response;
        }
    }
}
