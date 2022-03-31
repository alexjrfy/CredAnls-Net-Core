using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AnaliseController : Controller
    {
        // GET: AnaliseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AnaliseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AnaliseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnaliseController/Create
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

        // GET: AnaliseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AnaliseController/Edit/5
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

        // GET: AnaliseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AnaliseController/Delete/5
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
