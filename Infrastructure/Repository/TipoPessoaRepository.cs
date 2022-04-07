using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class TipoPessoaRepository : Repository<TipoPessoa>, ITipoPessoaRepository
    {
        public TipoPessoaRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
