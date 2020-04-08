using AspAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AspAPI.Controllers
{
    public class CoronaController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public CoronaController()
        {
            client.BaseAddress = new Uri("https://brmapi.azurewebsites.net/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET: Data
        public ActionResult Index()
        {
            return View(LoadCorona());
        }

        public JsonResult LoadCorona()
        {

            IEnumerable<CoronaViewModel> datas = null;
            var responseTask = client.GetAsync("Batches");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<CoronaViewModel>>();
                readTask.Wait();
                datas = readTask.Result;
            }
            else
            {
                datas = Enumerable.Empty<CoronaViewModel>();
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
        }




        // GET: Data/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Data/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Data/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Data/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Data/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Data/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Data/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
