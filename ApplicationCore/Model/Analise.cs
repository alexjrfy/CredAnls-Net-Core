using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Model
{
    public class Analise : Entity
    {
        [Column(TypeName = "VARCHAR(max)")]
        [Required(ErrorMessage = "Preencha o campo.")]
        public String Parecer { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExpiracao { get; set; }
        
        public Pessoa Pessoa { get; set; }
        public Motivo Motivo { get; set; }
        public Classificacao Classificacao { get; set; }

        [Required(ErrorMessage = "Preencha o campo.")]
        public Guid PessoaId { get; set; }
        [Required(ErrorMessage = "Preencha o campo.")]
        public Guid MotivoId { get; set; }
        [Required(ErrorMessage = "Preencha o campo.")]
        public Guid ClassificacaoId { get; set; }

    }
}
