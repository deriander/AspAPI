using API.Models;
using AspAPI.Models;
using AspAPI.Report;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public List<DepartmentModel> GetDepartmentList()
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

            return datas.ToList();
        }

        public ActionResult ExportToPDF()
        {
            DepartmentReport departmentReport = new DepartmentReport();
            byte[] abytes = departmentReport.PrepareReport(GetDepartmentList());
            return File(abytes, "application/pdf");
        }

        public ActionResult ExportToExcel()
        {

            var comlumHeadrs = new string[]
            {
                "Id",
                "Nama",
                "Tanggal Pembuatan",
                "Tanggal Diperbarui"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("Current Department"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                var j = 2;
                foreach (DepartmentModel dept in GetDepartmentList())
                {
                    worksheet.Cells["A" + j].Value = dept.Id;
                    worksheet.Cells["B" + j].Value = dept.Name;
                    worksheet.Cells["C" + j].Value = dept.CreateDate.ToString();
                    worksheet.Cells["D" + j].Value = dept.UpdateDate.ToString();
                  
                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Department_" + DateTime.Now + ".xlsx");
        }

        public ActionResult ExportToCSV()
        {
            var comlumHeadrs = new string[]
            {
                "Id",
                "Nama",
                "Tanggal Pembuatan",
                "Tanggal Diperbarui"
            };

            var employeeRecords = (from dept in GetDepartmentList()
                                   select new object[]
                                   {
                                            dept.Id,
                                            $"{dept.Name}",
                                            $"\"{dept.CreateDate.ToString()}\"", //Escaping ","
                                            dept.UpdateDate.ToString(),
                                   }).ToList();

            // Build the file content
            var departmentcsv = new StringBuilder();
            employeeRecords.ForEach(line =>
            {
                departmentcsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{departmentcsv.ToString()}");
            return File(buffer, "text/csv", $"Department_" + DateTime.Now + ".csv");

        }

        // GET: Data
        public ActionResult Index()
        {
            return View(LoadDepartment());
        }

    }
}