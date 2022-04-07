using ApplicationCore.Model;
using System.Collections.Generic;
using X.PagedList;

namespace Web.ViewModels
{
    public class AnaliseHistoricoViewModel
    {
        public IPagedList<Analise> Analises { get; set; }
        public List<TipoPessoa> TipoPessoa { get; set; }
        public List<Classificacao> Classificacao { get; set; }
        public List<Segmento> Segmentos { get; set; }
        public List<Motivo> Motivos { get; set; }
    }
}
