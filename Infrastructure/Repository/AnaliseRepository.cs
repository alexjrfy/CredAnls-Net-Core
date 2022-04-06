using ApplicationCore.AuxiliaryModel;
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
        public AnaliseRepository(ApplicationDbContext db) : base(db) 
        {
        }
        public async Task<List<Analise>> GetHistricoAnaliseLimite(int limite)
        {
            return await Db.Analise.Take(limite).AsNoTracking()
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo)
                .OrderByDescending(c => c.DataCadastro)
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
        public async Task<GroupCount> GetInfoAnaliseMotivo(string tipo)
        {
            var max = tipo == "hoje"
                ? Db.Analise.Where(x => x.DataCadastro == DateTime.Today).Select(x => x.Motivo.Id)
                : Db.Analise.Where(x => x.DataCadastro.Month == DateTime.Today.Month && x.DataCadastro.Year == DateTime.Today.Year).Select(x => x.Motivo.Id);

            var result = new GroupCount();
            if (max.Count() > 0)
            {
                result = await max.GroupBy(type => type).Select(x => new GroupCount{ Id = x.Key, Quantidade = x.Count() }).OrderByDescending(x => x.Quantidade).FirstAsync();
                
            }
            return result;
        }
        public async Task<GroupCount> GetInfoAnaliseClassificacao(int posicao)
        {

            var max = Db.Analise.Select(x => x.Classificacao.Id);

            var result = new GroupCount();
            if (max.Count() > 0)
            {
                var lista = await max.GroupBy(type => type).Select(x => new GroupCount { Id = x.Key, Quantidade = x.Count() }).OrderByDescending(x => x.Quantidade).ToListAsync();
                if (lista.Count>=posicao)
                {
                    result = lista.ElementAt(posicao - 1);
                }
            }
            return result;
        }

        public async Task<Analise> GetAnalisePorNumeroDocumento(double numeroDocumento, string chave)
        {
            return await Db.Analise.AsNoTracking()
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo).Where(c => c.Pessoa.NumeroDocumento == numeroDocumento && c.Pessoa.TipoPessoa.Chave==chave)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Analise>> GetHistricoAnalisePessoaLimite(Guid id, int limite)
        {
            return await Db.Analise.Take(limite).AsNoTracking()
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo)
                .Where(x => x.Pessoa.Id == id)
                .ToListAsync();
        }

        public async Task<Analise> GetAnaliseRecente(Guid id)
        {
            return await Db.Analise.AsNoTracking()
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo)
                .Where(x => x.Pessoa.Id == id).OrderByDescending(x => x.DataCadastro)
                .FirstOrDefaultAsync();
        }

        public async Task<Analise> GetAnaliseId(Guid id)
        {
            return await Db.Analise.AsNoTracking().Where(c => c.Id == id)
                .Include(c => c.Classificacao)
                .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
                .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
                .Include(c => c.Motivo)
                .FirstOrDefaultAsync();
        }
    }
}
