using API.Models;
using AspAPI.Models;
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
    public class DepartmentController : Controller
    {

        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5267/api/")
        };

        public JsonResult LoadDepartment()
        {

            IEnumerable<DepartmentModel> datas = null;
            var responseTask = client.GetAsync("Department");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DepartmentModel>>();
                readTask.Wait();
                datas = readTask.Result;
            }
            else
            {
                datas = Enumerable.Empty<DepartmentModel>();
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return new JsonResult { Data = datas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertOrEdit(DepartmentModel department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0)
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }           
            else
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Department");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DepartmentModel>>();
                var dept = data.FirstOrDefault(s => s.Id == Id);
                var json = JsonConvert.SerializeObject(dept, Formatting.None, new JsonSerializerSettings()
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
            return View(LoadDepartment());
        }

    }
}