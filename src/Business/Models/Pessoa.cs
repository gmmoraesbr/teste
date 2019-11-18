using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Pessoa : Entity
    {
        public string Nome { get; set; }

        public string Documento { get; set; }

        public DateTime DataNascimento { get; set; }

        public TipoPessoa TipoPessoa { get; set; }

        public Estacionamento Estacionamento { get; set; }
    }
}
