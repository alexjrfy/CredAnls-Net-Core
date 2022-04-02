using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class MotivoRepository : Repository<Motivo>, IMotivoRepository
    {
        public MotivoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
