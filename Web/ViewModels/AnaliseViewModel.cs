using ApplicationCore.Model;
using System;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class AnaliseViewModel
    {
        //Display
        public Pessoa Pessoa { get; set; }
        public List<Analise> Analises { get; set; }
        public Analise AnaliseRecente { get; set; }
        public List<Motivo> Motivos { get; set; }
        public List<Classificacao> Classificacoes { get; set; }
        public List<Grupo> Grupos { get; set; } 
        public List<Pessoa> PessoasGrupo { get; set; }

        //Var
        public Motivo Motivo { get; set; }
        public Classificacao Classificacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Parecer { get; set; }
        public Analise Analise { get; set; }
    }
}
