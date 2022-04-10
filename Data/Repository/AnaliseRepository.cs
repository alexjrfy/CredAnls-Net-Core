using Business.AuxiliaryModel;
using Business.Interfaces;
using Business.Model;
using Data.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Data.Repository
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

        public async Task<IPagedList<Analise>> GetTodasAnalisesPaginacao(int itensPorPagina, int numeroPagina, string tipoPessoaFiltro, double? numeroDocumento, Guid? classificacao, DateTime? dataInicio, DateTime? dataFim, Guid? segmento, Guid? motivo)
        {
            var lista = Db.Analise.Include(c => c.Classificacao)
            .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
            .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
            .Include(c => c.Motivo).OrderByDescending(c => c.DataCadastro).AsNoTracking();
            
            if (!String.IsNullOrEmpty(tipoPessoaFiltro))
            {
                lista = lista.Where(x => x.Pessoa.TipoPessoa.Chave.Contains(tipoPessoaFiltro));
            }

            if (numeroDocumento != null)
            {
                lista = lista.Where(x => x.Pessoa.NumeroDocumento == numeroDocumento);
            }

            if (classificacao != Guid.Empty)
            {
                lista = lista.Where(x => x.Classificacao.Id == classificacao);
            }

            if (segmento != Guid.Empty)
            {
                lista = lista.Where(x => x.Pessoa.Segmento.Id == segmento);
            }
            if (motivo != Guid.Empty)
            {
                lista = lista.Where(x => x.Motivo.Id == motivo);
            }
            if (dataInicio != null)
            {
                lista = lista.Where(x => x.DataCadastro.Date>=dataInicio);
            }
            if(dataFim != null)
            {
                lista = lista.Where(x => x.DataCadastro.Date<=dataFim);
            }


            return await lista.ToPagedListAsync(numeroPagina, itensPorPagina);
        }

        public async Task<List<Analise>> GetTodasAnalises(string tipoPessoaFiltro, double? numeroDocumento, Guid? classificacao, DateTime? dataInicio, DateTime? dataFim, Guid? segmento, Guid? motivo)
        {
            var lista = Db.Analise.Include(c => c.Classificacao)
            .Include(c => c.Pessoa).ThenInclude(c => c.TipoPessoa)
            .Include(c => c.Pessoa).ThenInclude(c => c.Segmento)
            .Include(c => c.Motivo).OrderByDescending(c => c.DataCadastro).AsNoTracking();

            if (!String.IsNullOrEmpty(tipoPessoaFiltro))
            {
                lista = lista.Where(x => x.Pessoa.TipoPessoa.Chave.Contains(tipoPessoaFiltro));
            }

            if (numeroDocumento != null)
            {
                lista = lista.Where(x => x.Pessoa.NumeroDocumento == numeroDocumento);
            }

            if (classificacao != Guid.Empty)
            {
                lista = lista.Where(x => x.Classificacao.Id == classificacao);
            }

            if (segmento != Guid.Empty)
            {
                lista = lista.Where(x => x.Pessoa.Segmento.Id == segmento);
            }
            if (motivo != Guid.Empty)
            {
                lista = lista.Where(x => x.Motivo.Id == motivo);
            }
            if (dataInicio != null)
            {
                lista = lista.Where(x => x.DataCadastro.Date >= dataInicio);
            }
            if (dataFim != null)
            {
                lista = lista.Where(x => x.DataCadastro.Date <= dataFim);
            }


            return await lista.ToListAsync();
        }
    }
}
