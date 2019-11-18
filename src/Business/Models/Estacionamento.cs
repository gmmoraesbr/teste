using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Estacionamento : Entity
    {
        public Guid PessoaId { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Placa { get; set; }

        public bool Manobrado { get; set; }

        public Pessoa Pessoa { get; set; }
    }
}
