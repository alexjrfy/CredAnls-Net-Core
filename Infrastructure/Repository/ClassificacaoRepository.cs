using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ClassificacaoRepository : Repository<Classificacao>, IClassificacaoRepository
    {
        public ClassificacaoRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Classificacao>> GetClassificacoes()
        {
            return await Db.Classificacao.OrderBy(x => x.Descricao).ToListAsync();
        }
    }
}
