using System;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Intefaces
{
    public interface IEstacionamentoRepository : IRepository<Estacionamento>
    {
        Task<Estacionamento> ObterEstabelicimento(Guid id);
        Task<Estacionamento> ObterEstabelicimentoComPessoa(Guid id);
        Task<Estacionamento> ObterEstabelicimentoPorPessoa(Guid id);
    }
}