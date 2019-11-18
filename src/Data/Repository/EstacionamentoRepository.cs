using System;
using System.Threading.Tasks;
using Business.Intefaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class EstacionamentoRepository : Repository<Estacionamento>, IEstacionamentoRepository
    {
        public EstacionamentoRepository(MeuDbContext context) : base(context) { }

        public async Task<Estacionamento> ObterEstabelicimento(Guid id)
        {
            return await Db.Estacionamentos.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Estacionamento> ObterEstabelicimentoComPessoa(Guid id)
        {
            return await Db.Estacionamentos.AsNoTracking()
                .Include(p => p.Pessoa)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Estacionamento> ObterEstabelicimentoPorPessoa(Guid id)
        {
            return await Db.Estacionamentos.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}