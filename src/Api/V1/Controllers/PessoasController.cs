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
    [Route("api/v{version:apiVersion}/pessoas")]
    public class PessoasController : MainController
    {
        private readonly IPessoaRepository _pesssoaRepository;
        private readonly IPessoaService _pessoaService;
        private readonly IEstacionamentoRepository _estacionamentoRepository;
        private readonly IMapper _mapper;

        public PessoasController(
            IPessoaRepository pesssoaRepository,
            IEstacionamentoRepository estacionamentoRepository,
            IPessoaService pessoaService,
            INotificador notificador,
            IMapper mapper,
            IUser user) : base(notificador, user)
        {
            _mapper = mapper;
            _pessoaService = pessoaService;
            _estacionamentoRepository = estacionamentoRepository;
            _pesssoaRepository = pesssoaRepository;
        }

        // GET: api/Pessoas
        [HttpGet]
        public async Task<IEnumerable<PessoaViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<PessoaViewModel>>(await _pesssoaRepository.ObterTodos());
        }

        // GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaViewModel>> GetPessoa([FromRoute] Guid id)
        {
            var pessoa = _mapper.Map<PessoaViewModel>(await _pesssoaRepository.ObterPessoa(id));

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        // PUT: api/Pessoas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, PessoaViewModel pessoaViewModel)
        {
            if (id != pessoaViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(pessoaViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pessoaService.Atualizar(_mapper.Map<Pessoa>(pessoaViewModel));

            return NoContent();
        }

        // POST: api/Pessoas
        [HttpPost]
        public async Task<ActionResult<PessoaViewModel>> Adicionar(PessoaViewModel pessoaViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pessoaService.Adicionar(_mapper.Map<Pessoa>(pessoaViewModel));

            return CustomResponse(pessoaViewModel);
        }

        // DELETE: api/Pessoas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PessoaViewModel>> Excluir([FromRoute] Guid id)
        {
            var pessoaViewModel = await _pesssoaRepository.ObterPessoa(id);

            if (pessoaViewModel == null) return NotFound();

            var estacionamento = await _estacionamentoRepository.ObterEstabelicimentoPorPessoa(id);

            if (estacionamento != null)
            {

                NotificarErro("Este manobrista não pode ser deletado.");
                return CustomResponse(pessoaViewModel);
            }

            try
            {
                await _pessoaService.Remover(id);
            }
            catch (Exception e)
            {

                throw;
            }

            return CustomResponse(pessoaViewModel);
        }
    }
}
