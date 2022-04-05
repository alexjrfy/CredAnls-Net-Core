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
        private readonly IMapper _mapper;
        public AnaliseController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository, IMapper mapper, IPessoaRepository pessoaRepository, IMotivoRepository motivoRepository, IClassificacaoRepository classificacaoRepository)
        {
            _logger = logger;
            _analiseRepository = analiseRespository;
            _pessoaRepository = pessoaRepository;
            _motivoRepository = motivoRepository;
            _classificacaoRepository = classificacaoRepository;
            _mapper = mapper;   
        }

        [Route("Analise/{tipoPessoa}/{numeroDocumento}")]
        public async Task<IActionResult> Details(string tipoPessoa, double numeroDocumento)
        {
            var típo            = tipoPessoa == "CPF" ? "F" : "J";

            var pessoa          = await _pessoaRepository.GetDadosPessoa(numeroDocumento, típo);
            var analises        = await _analiseRepository.GetHistricoAnalisePessoaLimite(pessoa.Id, 5);
            var analiseRecente  = await _analiseRepository.GetAnaliseRecente(pessoa.Id);

            var motivos         = await _motivoRepository.ObterTodos();
            var classificacoes  = await _classificacaoRepository.ObterTodos();

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

        //[Route("Analise/{tipoPessoa}/{numeroDocumento}")]
        [HttpPost]
        public async Task<IActionResult> Create(AnaliseViewModel analiseViewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction("Details", "Analise", new { tipoPessoa=analiseViewModel.Pessoa.TipoPessoa.Chave, numeroDocumento = analiseViewModel.Pessoa.NumeroDocumento,  });

            await _analiseRepository.Adicionar(_mapper.Map<Analise>(analiseViewModel.Analise));
            
            TempData["Success"] = "Análise de realizada com sucesso!";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string numeroDocumento, string chave)
        {
            // verificar se o CPF esta na base primeiro
            //Se existir, prossegue para a analise
            //Senão busca no externo
            var pessoa = await _pessoaRepository.ChecarPessoa(double.Parse(numeroDocumento), chave);

            if (pessoa == null)
            {
                TempData["Error"] = chave == "F" ? "CPF não Encontrado." : "CNPJ não encontrado.";
                return RedirectToAction("Index", "Home");
            }
            var url = pessoa.TipoPessoa.Chave=="F"?"CPF":"CNPJ";
            
            return RedirectToAction("Details", "Analise", new { tipoPessoa = url, numeroDocumento = pessoa.NumeroDocumento }); ;
        }



    }
}
