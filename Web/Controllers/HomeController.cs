using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnaliseRepository _analiseRepository;
        private readonly IMotivoRepository _motivoRepository;

        public HomeController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository, IMotivoRepository motivoRepository)
        {
            _logger = logger;
            _analiseRepository = analiseRespository;
            _motivoRepository = motivoRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var analise         = await _analiseRepository.GetHistricoAnaliseLimite(10);
            
            var quantidadeHoje  = await _analiseRepository.GetQuantidadesAnalisesHoje();
            var quantidadeMes   = await _analiseRepository.GetQuantidadesAnalisesMes();
            
            var motivoHoje      = await _analiseRepository.GetInfoAnaliseMotivo("hoje");
            var motivoMes       = await _analiseRepository.GetInfoAnaliseMotivo("mes");

            var aaa = motivoHoje.Id ;

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

            var homeViewModel = new HomeViewModel
            {
                Analises                = analise,
                QuantidateAnalisesHoje  = quantidadeHoje,
                QuantidateAnalisesMes   = quantidadeMes,
                QuantidadeMotivosHoje   = motivoHoje.Quantidade,
                NomeMotivosHoje         = nomeMotivoHoje,
                QuantidadeMotivosMes    = motivoMes.Quantidade,
                NomeMotivosMes          = nomeMotivoMes,

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
