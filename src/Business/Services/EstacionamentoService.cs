using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Intefaces;
using Business.Models;
using Business.Models.Validations;

namespace Business.Services
{
    public class EstacionamentoService : BaseService, IEstacionamentoService
    {
        private readonly IEstacionamentoRepository _estacionamentoRepository;
        
        public EstacionamentoService(IEstacionamentoRepository estacionamentoRepository, 
                                 INotificador notificador) : base(notificador)
        {
            _estacionamentoRepository = estacionamentoRepository;
        }

        public async Task<bool> Atualizar(Estacionamento estacionamento)
        {
            if (!ExecutarValidacao(new EstacionamentoValidation(), estacionamento)) return false;

            await _estacionamentoRepository.Atualizar(estacionamento);

            return true;
        }

        public void Dispose()
        {
            _estacionamentoRepository?.Dispose();
        }
    }
}