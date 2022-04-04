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
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<Pessoa> ChecarPessoa(double numeroDocumento, string chave)
        {
            return await Db.Pessoa.AsNoTracking()
                .Include(x => x.TipoPessoa)
                .Where(x => x.NumeroDocumento == numeroDocumento && x.TipoPessoa.Chave == chave)
                .FirstOrDefaultAsync();

        }

        public async Task<Pessoa> GetDadosPessoa(double numeroDocumento, string chave)
        {
            return await Db.Pessoa.AsNoTracking()
                .Include(t => t.TipoPessoa).Where(t => t.NumeroDocumento == numeroDocumento && t.TipoPessoa.Chave == chave)
                .Include(x => x.Grupo)
                .Include(x => x.Segmento)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Pessoa>> GetDadosPessoasGrupo(Guid idGrupo)
        {
            return await Db.Pessoa.AsNoTracking()
                .Include(t => t.TipoPessoa)
                .Include(x => x.Grupo).Where(g => g.Grupo.Id == idGrupo)
                .Include(x => x.Segmento)
                .ToListAsync();
        }
    }
}
