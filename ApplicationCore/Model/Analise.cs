using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Model
{
    public class Analise : Entity
    {
        [MaxLength]
        public String Parecer { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public Pessoa Pessoa { get; set; }
        public Motivo Motivo { get; set; }
        public Classificacao Classificacao { get; set; }

        public Guid PessoaId { get; set; }
        public Guid MotivoId { get; set; }
        public Guid ClassificacaoId { get; set; }

    }
}
