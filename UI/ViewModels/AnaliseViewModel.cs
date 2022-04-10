using Business.AuxiliaryModel;
using Business.Model;
using System;
using System.Collections.Generic;

namespace UI.ViewModels
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

        //Variables
        public Motivo Motivo { get; set; }
        public Classificacao Classificacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Parecer { get; set; }
        public Analise Analise { get; set; }

        public List<CheckedList> CheckedPessoas { get; set; }
    }
}
