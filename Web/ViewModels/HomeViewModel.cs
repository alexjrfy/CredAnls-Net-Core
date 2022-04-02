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
    }
}
