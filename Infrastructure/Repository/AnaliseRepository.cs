using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class AnaliseRepository : Repository<Analise>, IAnaliseRespository
    {
        public AnaliseRepository(ApplicationDbContext context) : base(context) { }
    }
}
