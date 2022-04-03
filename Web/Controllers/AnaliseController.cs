using ApplicationCore.Interfaces;
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
        public AnaliseController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository)
        {
            _logger = logger;
            _analiseRepository = analiseRespository;
        }

        [Route("analise-teste/{numeroDocumento}")]
        public async Task<IActionResult> Index()
        {
            
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Buscar(string numeroDocumento, string chave)
        {
            // verificar se o CPF esta na base primeiro
            //Se existir, prossegue para a analise
            //Senão busca no externo
            return View();
        }



    }
}
