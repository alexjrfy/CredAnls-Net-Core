using ApplicationCore.Interfaces;
using ApplicationCore.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AnaliseController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnaliseRepository _analiseRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IMotivoRepository _motivoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;
        private readonly ISegmentoRepository _segmentoRepository;
        private readonly ITipoPessoaRepository _tipoPessoaRepository;

        private readonly IMapper _mapper;
        public AnaliseController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository, IMapper mapper, IPessoaRepository pessoaRepository, IMotivoRepository motivoRepository, IClassificacaoRepository classificacaoRepository, ISegmentoRepository segmentoRepository, ITipoPessoaRepository tipoPessoaRepository)
        {
            _logger = logger;
            _analiseRepository = analiseRespository;
            _pessoaRepository = pessoaRepository;
            _motivoRepository = motivoRepository;
            _classificacaoRepository = classificacaoRepository;
            _segmentoRepository = segmentoRepository;
            _tipoPessoaRepository = tipoPessoaRepository;
            _mapper = mapper;   
        }

        [Route("Analise/{tipoPessoa}/{numeroDocumento}")]
        public async Task<IActionResult> Create(string tipoPessoa, double numeroDocumento)
        {
            var tipo            = tipoPessoa == "CPF" ? "F" : "J";

            var pessoa          = await _pessoaRepository.GetDadosPessoa(numeroDocumento, tipo);
            var analises        = await _analiseRepository.GetHistricoAnalisePessoaLimite(pessoa.Id, 5);
            var analiseRecente  = await _analiseRepository.GetAnaliseRecente(pessoa.Id);

            var motivos         = await _motivoRepository.ObterTodos();
            var classificacoes  = await _classificacaoRepository.GetClassificacoes();

            var pessoasGrupo    = await _pessoaRepository.GetDadosPessoasGrupo(pessoa.Grupo.Id);

            var analiseViewModel = new AnaliseViewModel
            {
                Pessoa = pessoa,
                Analises = analises,
                AnaliseRecente = analiseRecente,
                Motivos = motivos,
                Classificacoes = classificacoes,
                PessoasGrupo = pessoasGrupo,
            };

            return View(analiseViewModel);
        }

        [Route("Analise/{tipoPessoa}/{numeroDocumento}")]
        [HttpPost]
        public async Task<IActionResult> Create(AnaliseViewModel analiseViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Erro ao salvar a análise.";
                return RedirectToAction("Create", "Analise", new { tipoPessoa = analiseViewModel.Pessoa.TipoPessoa.Chave, numeroDocumento = analiseViewModel.Pessoa.NumeroDocumento, });
            }

            await _analiseRepository.Adicionar(_mapper.Map<Analise>(analiseViewModel.Analise));

            foreach ( var item in analiseViewModel.CheckedPessoas)
            {
                if (item.Id != analiseViewModel.Analise.PessoaId && item.IsChecked == true)
                {
                    analiseViewModel.Analise.Id = new Guid();
                    analiseViewModel.Analise.PessoaId = item.Id;
                    await _analiseRepository.Adicionar(_mapper.Map<Analise>(analiseViewModel.Analise));
                }
                
            }
            
            
            TempData["Success"] = "Análise de realizada com sucesso!";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string numeroDocumento, string chave)
        {
            var pessoa = await _pessoaRepository.ChecarPessoa(double.Parse(numeroDocumento), chave);

            if (pessoa == null)
            {
                TempData["Error"] = chave == "F" ? "CPF não Encontrado." : "CNPJ não encontrado.";
                return RedirectToAction("Index", "Home");
            }
            var url = pessoa.TipoPessoa.Chave=="F"?"CPF":"CNPJ";
            
            return RedirectToAction("Create", "Analise", new { tipoPessoa = url, numeroDocumento = pessoa.NumeroDocumento }); ;
        }
        
        [Route("Analise/Detalhes/{guid}")]
        public async Task<IActionResult> Details(Guid guid)
        {
            var analise = await _analiseRepository.GetAnaliseId(guid);
            if (analise == null)
            {
                TempData["Error"] = "Registro não encontrado";
                return RedirectToAction("Index", "Home");
            }

            var analiseDetailsViewModel = new AnaliseDetailsViewModel
            {
                Analise = analise,
            };

            return View(analiseDetailsViewModel);
        }

        [Route("Analise/Historico")]
        public async Task<IActionResult> Historico(int? page, string tipoPessoaFiltro, double? numeroDocumento, Guid classificacao, DateTime? dataInicio, DateTime? dataFim, Guid segmento, Guid motivo)
        {
            const int itensPorPagina = 10;
            int numeroPagina = (page ?? 1);
            
            var lista = await _analiseRepository.GetTodasAnalisesPaginacao(itensPorPagina, numeroPagina, tipoPessoaFiltro, numeroDocumento, classificacao, dataInicio, dataFim, segmento, motivo);
            var motivos = await _motivoRepository.ObterTodos();
            var classificacoes = await _classificacaoRepository.GetClassificacoes();
            var segmentos = await _segmentoRepository.ObterTodos();
            var tipoPessoa = await _tipoPessoaRepository.ObterTodos();

            var analiseHistoricoViewModel = new AnaliseHistoricoViewModel
            {
                Analises = lista,
                Motivos = motivos,
                Classificacao = classificacoes,
                Segmentos = segmentos,
                TipoPessoa = tipoPessoa
            };

            return View(analiseHistoricoViewModel);
        }

        public async Task<IActionResult> ExportarExcel(string tipoPessoaFiltro, double? numeroDocumento, Guid classificacao, DateTime? dataInicio, DateTime? dataFim, Guid segmento, Guid motivo)
        {
            var analises = await _analiseRepository.GetTodasAnalises(tipoPessoaFiltro, numeroDocumento, classificacao, dataInicio, dataFim, segmento, motivo);
            return View();
        }
    }
}
