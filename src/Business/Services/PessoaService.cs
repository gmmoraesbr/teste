using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Intefaces;
using Business.Models;
using Business.Models.Validations;

namespace Business.Services
{
    public class PessoaService : BaseService, IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        
        public PessoaService(IPessoaRepository pessoaRepository, 
                                 INotificador notificador) : base(notificador)
        {
            _pessoaRepository = pessoaRepository;
        }

        public async Task<bool> Adicionar(Pessoa pessoa)
        {
            if (!ExecutarValidacao(new PessoaValidation(), pessoa) ) return false;

            if (_pessoaRepository.Buscar(f => f.Documento == pessoa.Documento).Result.Any())
            {
                Notificar("Já existe uma pessoa com este documento informado.");
                return false;
            }

            await _pessoaRepository.Adicionar(pessoa);
            return true;
        }

        public async Task<bool> Atualizar(Pessoa pessoa)
        {
            if (!ExecutarValidacao(new PessoaValidation(), pessoa)) return false;

            if (_pessoaRepository.Buscar(f => f.Documento == pessoa.Documento && f.Id != pessoa.Id).Result.Any())
            {
                Notificar("Já existe uma pessoa com este documento infomado.");
                return false;
            }

            await _pessoaRepository.Atualizar(pessoa);
            return true;
        }


        public async Task<bool> Remover(Guid id)
        {
            await _pessoaRepository.Remover(id);
            return true;
        }

        public void Dispose()
        {
            _pessoaRepository?.Dispose();
        }
    }
}