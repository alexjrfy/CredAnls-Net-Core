using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class SegmentoRepository : Repository<Segmento>, ISegmentoRepository
    {
        public SegmentoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
