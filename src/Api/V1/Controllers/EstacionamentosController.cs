using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Models;
using Data.Context;
using Api.Controllers;
using Business.Intefaces;
using AutoMapper;
using Api.ViewModels;

namespace Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/estacionamentos")]
    public class EstacionamentoController : MainController
    {
        private readonly IEstacionamentoRepository _estacionamentoRepository;
        private readonly IEstacionamentoService _estacionamentoService;
        private readonly IMapper _mapper;

        public EstacionamentoController(
            IEstacionamentoRepository estacionamentoRepository,
            IEstacionamentoService estacionamentoService,
            INotificador notificador,
            IMapper mapper,
            IUser user) : base(notificador, user)
        {
            _mapper = mapper;
            _estacionamentoService = estacionamentoService;
            _estacionamentoRepository = estacionamentoRepository;
        }

        // GET: api/Estacionamentos
        [HttpGet]
        public async Task<IEnumerable<EstacionamentoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<EstacionamentoViewModel>>(await _estacionamentoRepository.ObterTodos());
        }

        // GET: api/Estacionamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstacionamentoViewModel>> GetEstacionamento([FromRoute] Guid id)
        {
            var estacionamento = _mapper.Map<EstacionamentoViewModel>(await _estacionamentoRepository.ObterEstabelicimento(id));

            if (estacionamento == null)
            {
                return NotFound();
            }

            return estacionamento;
        }

        // PUT: api/Estacionamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, EstacionamentoViewModel estabelecimentoViewModel)
        {
            if (id != estabelecimentoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(estabelecimentoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _estacionamentoService.Atualizar(_mapper.Map<Estacionamento>(estabelecimentoViewModel));

            return NoContent();
        }

        // POST: api/Estacionamentos
        [HttpPost]
        public async Task<ActionResult<EstacionamentoViewModel>> Adicionar(EstacionamentoViewModel estabelecimentoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _estacionamentoRepository.Adicionar(_mapper.Map<Estacionamento>(estabelecimentoViewModel));

            return CustomResponse(estabelecimentoViewModel);
        }

        // DELETE: api/Estacionamentos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EstacionamentoViewModel>> Excluir([FromRoute] Guid id)
        {
            var estabelecimentoViewModel = await _estacionamentoRepository.ObterEstabelicimento(id);

            if (estabelecimentoViewModel == null) return NotFound();

            await _estacionamentoRepository.Remover(id);

            return CustomResponse(estabelecimentoViewModel);
        }
    }
}
