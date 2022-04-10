using Business.Interfaces;
using Business.Model;
using Data.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class MotivoRepository : Repository<Motivo>, IMotivoRepository
    {
        public MotivoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
