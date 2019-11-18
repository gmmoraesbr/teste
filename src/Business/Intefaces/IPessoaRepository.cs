using System;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Intefaces
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<Pessoa> ObterPessoa(Guid id);
        //Task<Pessoa> ObterFornecedorProdutosEndereco(Guid id);
    }
}