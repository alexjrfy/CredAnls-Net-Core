using ApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAnaliseRepository : IRepository<Analise>
    {
        Task<List<Analise>> GetHistricoAnaliseLimite(int limite);
        Task<int> GetQuantidadesAnalisesHoje();

        Task<int> GetQuantidadesAnalisesMes();

        Task<int> GetQuantidadeMotivosHoje();
        Task<Guid> GetMotivoIdHoje();
        Task<int> GetQuantidadeMotivosMes();
        Task<Guid> GetMotivoIdMes();
        
    }
}