using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class ClassificacaoRepository : Repository<Classificacao>, IClassificacaoRepository
    {
        public ClassificacaoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
