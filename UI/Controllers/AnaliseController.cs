using Business.Interfaces;
using Business.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UI.ViewModels;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace UI.Controllers
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AnaliseController(ILogger<HomeController> logger, IAnaliseRepository analiseRespository, IMapper mapper, IPessoaRepository pessoaRepository, IMotivoRepository motivoRepository, IClassificacaoRepository classificacaoRepository, ISegmentoRepository segmentoRepository, ITipoPessoaRepository tipoPessoaRepository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _analiseRepository = analiseRespository;
            _pessoaRepository = pessoaRepository;
            _motivoRepository = motivoRepository;
            _classificacaoRepository = classificacaoRepository;
            _segmentoRepository = segmentoRepository;
            _tipoPessoaRepository = tipoPessoaRepository;
            _mapper = mapper;   
            _webHostEnvironment = webHostEnvironment;
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

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Análise");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Número do Documento";
                worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 1).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 2).Value = "Tipo";
                worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 2).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 3).Value = "Segmento";
                worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 3).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 4).Value = "Recebido em:";
                worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 4).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 5).Value = "Atualizado em:";
                worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 5).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 6).Value = "Classificação";
                worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 6).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 7).Value = "Data de Expiração";
                worksheet.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 7).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 8).Value = "Motivo";
                worksheet.Cell(currentRow, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 8).RichText.SetFontColor(XLColor.White); ;

                worksheet.Cell(currentRow, 9).Value = "Parecer";
                worksheet.Cell(currentRow, 9).Style.Fill.BackgroundColor = XLColor.FromArgb(27, 77, 62);
                worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(currentRow, 9).RichText.SetFontColor(XLColor.White); ;

                for (int i = 0; i < analises.Count; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = analises[i].Pessoa.NumeroDocumento ;
                        worksheet.Cell(currentRow, 1).SetDataType(XLDataType.Text);
                        worksheet.Cell(currentRow, 2).Value = analises[i].Pessoa.TipoPessoa.Chave;
                        worksheet.Cell(currentRow, 3).Value = analises[i].Pessoa.Segmento.Descricao;
                        worksheet.Cell(currentRow, 4).Value = analises[i].Pessoa.DataCadastro;
                        worksheet.Cell(currentRow, 5).Value = analises[i].DataCadastro;
                        worksheet.Cell(currentRow, 6).Value = analises[i].Classificacao.Descricao;
                        worksheet.Cell(currentRow, 7).Value = analises[i].DataExpiracao;
                        worksheet.Cell(currentRow, 8).Value = analises[i].Motivo.Descricao;
                        worksheet.Cell(currentRow, 9).Value = Regex.Replace(analises[i].Parecer, "<.*?>", String.Empty);

                    }
                }
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=Análises.xlsx");
                Response.ContentType = "application/xls";
                Response.Body.WriteAsync(content);
                Response.Body.Flush();


                return View();
            }
        }

        public async Task<IActionResult> ExportarPDF(string tipoPessoaFiltro, double? numeroDocumento, Guid classificacao, DateTime? dataInicio, DateTime? dataFim, Guid segmento, Guid motivo)
        {
            var analises = await _analiseRepository.GetTodasAnalises(tipoPessoaFiltro, numeroDocumento, classificacao, dataInicio, dataFim, segmento, motivo);

            var path = _webHostEnvironment.WebRootPath;

            if (analises.Count > 0)
            {
                int pdfRowIndex = 1;
                string filename = "Análises-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
                string filepath = Path.Combine(path+"/files/pdfs/", filename + ".pdf");
                Document document = new Document();
                document.SetPageSize(PageSize.A4.Rotate());
                FileStream fs = new FileStream(filepath, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10, BaseColor.WHITE);
                Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);

                float[] columnDefinitionSize = { 5F, 2F, 3F, 4F, 4F, 2F, 4F, 5F, 5F };
                PdfPTable table;
                PdfPCell cell;

                table = new PdfPTable(columnDefinitionSize)
                {
                    WidthPercentage = 100
                };

                cell = new PdfPCell
                {
                    BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
                };

                PdfPCell cell1 = new PdfPCell(new Phrase("Número do Documento", font1));
                cell1.BackgroundColor = new BaseColor(27, 77, 62);
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell2 = new PdfPCell(new Phrase("Tipo", font1));
                cell2.BackgroundColor = new BaseColor(27, 77, 62);
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell3 = new PdfPCell(new Phrase("Segmento", font1));
                cell3.BackgroundColor = new BaseColor(27, 77, 62);
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell4 = new PdfPCell(new Phrase("Recebido em:", font1));
                cell4.BackgroundColor = new BaseColor(27, 77, 62);
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell5 = new PdfPCell(new Phrase("Atualizado em:", font1));
                cell5.BackgroundColor = new BaseColor(27, 77, 62);
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell6 = new PdfPCell(new Phrase("Classificação", font1));
                cell6.BackgroundColor = new BaseColor(27, 77, 62);
                cell6.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell7 = new PdfPCell(new Phrase("Data de Expiração", font1));
                cell7.BackgroundColor = new BaseColor(27, 77, 62);
                cell7.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell8 = new PdfPCell(new Phrase("Motivo", font1));
                cell8.BackgroundColor = new BaseColor(27, 77, 62);
                cell8.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell9 = new PdfPCell(new Phrase("Parecer", font1));
                cell9.BackgroundColor = new BaseColor(27, 77, 62);
                cell9.HorizontalAlignment = Element.ALIGN_CENTER;

                table.AddCell(cell1);
                table.AddCell(cell2);
                table.AddCell(cell3);
                table.AddCell(cell4);
                table.AddCell(cell5);
                table.AddCell(cell6);
                table.AddCell(cell7);
                table.AddCell(cell8);
                table.AddCell(cell9);

                table.HeaderRows = 1;

                foreach (var data in analises)
                {
                    
                    table.AddCell(new Phrase(data.Pessoa.NumeroDocumento.ToString(), font2));
                    table.AddCell(new Phrase(data.Pessoa.TipoPessoa.Chave.ToString(), font2));
                    table.AddCell(new Phrase(data.Pessoa.Segmento.Descricao.ToString(), font2));
                    table.AddCell(new Phrase(data.Pessoa.DataCadastro.ToString(), font2));
                    table.AddCell(new Phrase(data.DataCadastro.ToString(), font2));
                    table.AddCell(new Phrase(data.Classificacao.Descricao.ToString(), font2));
                    table.AddCell(new Phrase(data.DataExpiracao.ToString(), font2));
                    table.AddCell(new Phrase(data.Motivo.Descricao.ToString(), font2));
                    table.AddCell(new Phrase(Regex.Replace(data.Parecer, "<.*?>", String.Empty), font2));

                    pdfRowIndex++;
                }

                document.Add(table);
                document.Close();
                document.CloseDocument();
                document.Dispose();
                writer.Close();
                writer.Dispose();
                fs.Close();
                fs.Dispose();

                FileStream sourceFile = new FileStream(filepath, FileMode.Open);
                float fileSize = 0;
                fileSize = sourceFile.Length;
                byte[] getContent = new byte[Convert.ToInt32(Math.Truncate(fileSize))];
                sourceFile.Read(getContent, 0, Convert.ToInt32(sourceFile.Length));
                sourceFile.Close();
                Response.Clear();
                Response.Headers.Clear();
                Response.ContentType = "application/pdf";
                Response.Headers.Add("Content-Length", getContent.Length.ToString());
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + filename + ".pdf;");
                Response.Body.WriteAsync(getContent);
                Response.Body.Flush();
                System.IO.File.Delete(filepath);
            }
            return View();
        }
    }
}


