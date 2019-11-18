using System;
using System.Threading.Tasks;
using Business.Intefaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task<Pessoa> ObterPessoa(Guid id)
        {
            return await Db.Pessoas.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}