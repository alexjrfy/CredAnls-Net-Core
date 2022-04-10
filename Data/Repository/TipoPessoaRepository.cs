using Business.Interfaces;
using Business.Model;
using Data.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class TipoPessoaRepository : Repository<TipoPessoa>, ITipoPessoaRepository
    {
        public TipoPessoaRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
