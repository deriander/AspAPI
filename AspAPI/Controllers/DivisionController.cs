using API.Models;
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

        public List<DivisionViewModel> GetDivisionList()
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

            return datas.ToList();
        }

        public ActionResult ExportToPDF()
        {
            DivisionReport divisionReport = new DivisionReport();            
            byte[] abytes = divisionReport.PrepareReport(GetDivisionList());
            return File(abytes, "application/pdf");
        }

        public ActionResult ExportToExcel()
        {

            var comlumHeadrs = new string[]
            {
                "Id",
                "Nama Divisi",
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Diperbarui"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("Current Division"); //Worksheet name
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
                foreach (DivisionViewModel div in GetDivisionList())
                {
                    worksheet.Cells["A" + j].Value = div.Id;
                    worksheet.Cells["B" + j].Value = div.DivisionName;
                    worksheet.Cells["C" + j].Value = div.DepartmentName;
                    worksheet.Cells["D" + j].Value = div.CreateDate.ToString();
                    worksheet.Cells["E" + j].Value = div.UpdateDate.ToString();

                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Division_" + DateTime.Now + ".xlsx");
        }

        public ActionResult ExportToCSV()
        {
            var comlumHeadrs = new string[]
            {
                "Id",
                "Nama Divisi",
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Diperbarui"
            };

            var employeeRecords = (from division in GetDivisionList()
                                   select new object[]
                                   {
                                            division.Id,
                                            $"{division.DivisionName}",
                                            $"\"{division.DepartmentName}\"", //Escaping ","
                                            $"\"{division.CreateDate.ToString()}\"", //Escaping ","
                                            division.UpdateDate.ToString(),
                                   }).ToList();

            // Build the file content
            var divisioncsv = new StringBuilder();
            employeeRecords.ForEach(line =>
            {
                divisioncsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{divisioncsv.ToString()}");
            return File(buffer, "text/csv", $"Division_" + DateTime.Now + ".csv");

        }

        // GET: Data
        public ActionResult Index()
        {
            return View(LoadDivision());
        }
    }
}