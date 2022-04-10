using Business.Interfaces;
using Business.Model;
using Data.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class GrupoRepository : Repository<Grupo>, IGrupoRepository
    {
        public GrupoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
