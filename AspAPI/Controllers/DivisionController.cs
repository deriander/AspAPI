using API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspAPI.Controllers
{
    public class DivisionController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5267/api/")
        };

        public JsonResult LoadDivision()
        {

            IEnumerable<DivisionViewModel> datas = null;
            var responseTask = client.GetAsync("Division");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisionViewModel>>();
                readTask.Wait();
                datas = readTask.Result;
            }
            else
            {
                datas = Enumerable.Empty<DivisionViewModel>();
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return new JsonResult { Data = datas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertOrEdit(DivisionModel division)
        {
            var myContent = JsonConvert.SerializeObject(division);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (division.Id == 0)
            {
                var result = client.PostAsync("Division", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var result = client.PutAsync("Division/" + division.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Division/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Division");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DivisionViewModel>>();
                var div = data.FirstOrDefault(s => s.Id == Id);
                var json = JsonConvert.SerializeObject(div, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal server error");
        }

        // GET: Data
        public ActionResult Index()
        {
            return View(LoadDivision());
        }
    }
}