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
    public class AnaliseRepository : Repository<Analise>, IAnaliseRepository
    {
        public AnaliseRepository(ApplicationDbContext context) : base(context) 
        {
        }
        public async Task<List<Analise>> GetHistricoAnaliseLimite(int limite)
        {
            return await Db.Analise.Take(limite).AsNoTracking()
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo)
                .ToListAsync();
        }

        public async Task<int> GetQuantidadesAnalisesHoje()
        {
            return await Db.Analise.CountAsync(x => x.DataCadastro == DateTime.Today);
        }

        public async Task<int> GetQuantidadesAnalisesMes()
        {
            return await Db.Analise.CountAsync(x => x.DataCadastro.Month == DateTime.Today.Month && x.DataCadastro.Year == DateTime.Today.Year);
        }

        public async Task<int> GetQuantidadeMotivosHoje()
        {
            var quantidade = await GetMaxMotivo("hoje");
            return quantidade.Count;
        }

        public async Task<Guid> GetMotivoIdHoje()
        {
            var guid = await GetMaxMotivo("hoje");
            return  guid.Group;
        }

        public async Task<int> GetQuantidadeMotivosMes()
        {
            var quantidade = await GetMaxMotivo("mes");
            return quantidade.Count;
        }

        public async Task<Guid> GetMotivoIdMes()
        {
            var guid = await GetMaxMotivo("mes");
            return guid.Group;
        }

        public async Task<dynamic> GetMaxMotivo(string tipo)
        {
            var max = tipo == "hoje"
                ? Db.Analise.Where(x => x.DataCadastro == DateTime.Today).Select(x => x.Motivo.Id)
                : Db.Analise.Where(x => x.DataCadastro.Month == DateTime.Today.Month && x.DataCadastro.Year == DateTime.Today.Year).Select(x => x.Motivo.Id);

            dynamic result = new { Group = Guid.Empty, Count = 0 };
            if (max.Count() > 0)
            {
                result = await max.GroupBy(type => type).Select(x => new { Group = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).FirstAsync();
                
            }
            return result;
        }

    }
}
