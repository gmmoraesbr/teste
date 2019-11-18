using System;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Intefaces
{
    public interface IEstacionamentoService : IDisposable
    {
        Task<bool> Atualizar(Estacionamento estacionamento);
    }
}