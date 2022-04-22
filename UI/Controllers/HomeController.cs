using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UI.Models;
using UI.ViewModels;
using Business.Interfaces;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnaliseRepository _analiseRepository;
        private readonly IMotivoRepository _motivoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;

        public HomeController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository, IMotivoRepository motivoRepository, IClassificacaoRepository classificacaoRepository)
        {
            _logger                     = logger;
            _analiseRepository          = analiseRespository;
            _motivoRepository           = motivoRepository;
            _classificacaoRepository    = classificacaoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var analise         = await _analiseRepository.GetHistricoAnaliseLimite(10);
            
            var quantidadeHoje  = await _analiseRepository.GetQuantidadesAnalisesHoje();
            var quantidadeMes   = await _analiseRepository.GetQuantidadesAnalisesMes();
            
            var motivoHoje      = await _analiseRepository.GetInfoAnaliseMotivo("hoje");
            var motivoMes       = await _analiseRepository.GetInfoAnaliseMotivo("mes");

            var nomeMotivoHoje = "";
            if (motivoHoje.Id != Guid.Empty)
            {
                var nomeMotivoHojeBusca = await _motivoRepository.ObterPorId(motivoHoje.Id);
                nomeMotivoHoje = nomeMotivoHojeBusca.Descricao;
            }

            var nomeMotivoMes = "";
            if (motivoMes.Id != Guid.Empty)
            {
                var nomeMotivoMesBusca = await _motivoRepository.ObterPorId(motivoMes.Id);
                nomeMotivoMes = nomeMotivoMesBusca.Descricao;
            }

            var ouro   = await _analiseRepository.GetInfoAnaliseClassificacao(1);
            var prata  = await _analiseRepository.GetInfoAnaliseClassificacao(2);
            var bronze = await _analiseRepository.GetInfoAnaliseClassificacao(3);

            var nomeOuro = "";
            if(ouro.Id != Guid.Empty)
            {
                var classificacaoOuro = await _classificacaoRepository.ObterPorId(ouro.Id);
                nomeOuro = classificacaoOuro.Descricao;
            }

            var nomePrata = "";
            if (prata.Id != Guid.Empty)
            {
                var classificacaoPrata = await _classificacaoRepository.ObterPorId(prata.Id);
                nomePrata = classificacaoPrata.Descricao;
            }

            var nomeBronze = "";
            if (bronze.Id != Guid.Empty)
            {
                var classificacaoBronze = await _classificacaoRepository.ObterPorId(bronze.Id);
                nomeBronze = classificacaoBronze.Descricao;
            }

            var homeViewModel = new HomeViewModel
            {
                Analises                = analise,
                QuantidateAnalisesHoje  = quantidadeHoje,
                QuantidateAnalisesMes   = quantidadeMes,
                QuantidadeMotivosHoje   = motivoHoje.Quantidade,
                NomeMotivosHoje         = nomeMotivoHoje,
                QuantidadeMotivosMes    = motivoMes.Quantidade,
                NomeMotivosMes          = nomeMotivoMes,
                QuantidadeOuro          = ouro.Quantidade,
                ClassificacaoOuro       = nomeOuro,
                QuantidadePrata         = prata.Quantidade,
                ClassificacaoPrata      = nomePrata,
                QuantidadeBronze        = bronze.Quantidade,
                ClassificacaoBronze     = nomeBronze,


            };
            
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
