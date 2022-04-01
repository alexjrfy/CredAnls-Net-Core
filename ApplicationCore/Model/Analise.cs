using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class Analise : Entity
    {
        public string Parecer { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public Pessoa Pessoa { get; set; }
        public Motivo Motivo { get; set; }
        public Classificacao Classificacao { get; set; }

    }
}
