using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Model
{
    public class Pessoa : Entity
    {
        public double NumeroDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool BloqueadoAnalise { get; set; }
        public Segmento Segmento { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public Grupo Grupo { get; set; }


    }
}
