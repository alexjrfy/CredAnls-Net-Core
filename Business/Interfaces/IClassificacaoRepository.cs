using Business.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IClassificacaoRepository : IRepository<Classificacao>
    {
        Task<List<Classificacao>> GetClassificacoes();
    }
}
