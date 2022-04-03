using ApplicationCore.Model;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class HomeViewModel
    {
        public List<Analise> Analises { get; set; }
        public int QuantidateAnalisesHoje { get; set; }
        public int QuantidateAnalisesMes { get; set; }
        public int QuantidadeMotivosHoje { get; set; }
        public string NomeMotivosHoje { get; set; }
        public int QuantidadeMotivosMes { get; set; }
        public string NomeMotivosMes { get; set; }
        public int QuantidadeOuro { get; set; }
        public string ClassificacaoOuro { get; set; }
        public int QuantidadePrata { get; set; }
        public string ClassificacaoPrata { get; set; }
        public int QuantidadeBronze { get; set; }
        public string ClassificacaoBronze { get; set; }
    }
}
