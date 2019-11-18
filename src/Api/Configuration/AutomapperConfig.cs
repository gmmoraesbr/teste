using AutoMapper;
using Api.ViewModels;
using Business.Models;

namespace Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Pessoa, PessoaViewModel>().ReverseMap();

            CreateMap<Estacionamento, EstacionamentoViewModel>().ForMember(p => p.Pessoa, opt => opt.Ignore());

        }
    }
}