using ApplicationCore.AuxiliaryModel;
using ApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ApplicationCore.Interfaces
{
    public interface IAnaliseRepository : IRepository<Analise>
    {
        Task<List<Analise>> GetHistricoAnaliseLimite(int limite);
        Task<List<Analise>> GetHistricoAnalisePessoaLimite(Guid id, int limite);
        Task<Analise> GetAnaliseRecente(Guid id);
        Task<int> GetQuantidadesAnalisesHoje();
        Task<int> GetQuantidadesAnalisesMes();
        Task<GroupCount> GetInfoAnaliseMotivo(string periodo);
        Task<GroupCount> GetInfoAnaliseClassificacao(int posicao);
        Task<Analise> GetAnalisePorNumeroDocumento(double numeroDocumento, string chave);
        Task<Analise> GetAnaliseId(Guid id);
        Task<IPagedList<Analise>> GetTodasAnalisesPaginacao(int itensPorPagina, int numeroPagina, string tipoPessoaFiltro, double? numeroDocumento, Guid? classificacao, DateTime? dataInicio, DateTime? dataFim, Guid? segmento, Guid? motivo);
        Task<List<Analise>> GetTodasAnalises(string tipoPessoaFiltro, double? numeroDocumento, Guid? classificacao, DateTime? dataInicio, DateTime? dataFim, Guid? segmento, Guid? motivo);

    }
}