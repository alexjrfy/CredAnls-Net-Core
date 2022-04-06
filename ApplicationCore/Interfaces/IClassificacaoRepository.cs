using ApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IClassificacaoRepository : IRepository<Classificacao>
    {
        Task<List<Classificacao>> GetClassificacoes();
    }
}
