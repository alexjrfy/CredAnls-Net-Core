using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HistoricoController : Controller
    {
        // GET: HistoricoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HistoricoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HistoricoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HistoricoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HistoricoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HistoricoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HistoricoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HistoricoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
