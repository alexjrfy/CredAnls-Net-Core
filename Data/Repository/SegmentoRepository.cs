using Business.Interfaces;
using Business.Model;
using Data.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class SegmentoRepository : Repository<Segmento>, ISegmentoRepository
    {
        public SegmentoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
