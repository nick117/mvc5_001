using System;
using System.IO;
using System.Net;
using appraisal.Models;
using appraisal.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using appraisal.Filters;
using PagedList;

namespace appraisal.Controllers
{
    [AuthorizeAD(Groups = "webAdmin01")]
    public class ImportTsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ImportTs
        public ActionResult Index(int? page)
        {
            var query = db.importtss
                            .OrderBy(x => x.CardNo);

            var result = query.ToPagedList(pageNumber: page ?? 1, pageSize: 20);

            return View(result);
        }
        public ActionResult Details(string id, int? page)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTs its = db.importtss.Find(id);
            if (its == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentPage = page;
            return View(its);
        }

        public ActionResult UpdateEmp(int? page)
        {
            ViewBag.CurrentPage = page;
            return View();
        }

        [HttpPost, ActionName("UpdateEmp")]
        public ActionResult UpdateEmpConfirmed(int? page)
        {
            string newTitle;
            string newGroup;
            int newDep = 0;
            var find = from data in db.importtss
                       where data.CardNo1 != null
                       select data;
            foreach (var item in find)
            {
                newTitle = String.IsNullOrEmpty(item.MTitle) ? item.PTitle : item.MTitle;
                newGroup = String.IsNullOrEmpty(item.Group) ? "" : item.Group;
                var deps = db.deps.Where(s => s.title.Contains(item.Depart.Trim()) && s.title.Contains(newGroup)).FirstOrDefault();
                if (deps != null)
                {
                        newDep = deps.id;
                }
                else
                { newDep = 0; }
                var empq = db.emps.Where(s => s.eid.ToUpper().Equals(item.CardNo.ToUpper())).FirstOrDefault();
                if (empq != null)
                {
                    empq.title = newTitle;
//                    emp1.dept = (newDep == 0) ? emp1.dept : newDep;
                    db.Entry(empq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    emp empa = new emp();
                    empa.eid = item.CardNo;
                    empa.cname = item.Name;
                    empa.title = newTitle;
                    empa.dept = (newDep == 0) ? 1 : newDep;                 
                    db.emps.Add(empa);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", new { page = page });
        }


        private string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if (file == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳檔案!");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }
            if (file.ContentLength <= 0)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳正確的檔案.");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(file.FileName).ToLower();

            if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                &&
                !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳 .xls 或 .xlsx 格式的檔案");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(file);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");

                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        /// <summary>
        /// Files the upload handler.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file;上傳失敗：沒有檔案！</exception>
        /// <exception cref="System.InvalidOperationException">上傳失敗：檔案沒有內容！</exception>
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                throw new ArgumentNullException("file", "上傳失敗：沒有檔案！");
            }
            if (file.ContentLength <= 0)
            {
                throw new InvalidOperationException("上傳失敗：檔案沒有內容！");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [HttpPost]
        public ActionResult Import(string savedFileName)
        {
            var jo = new JObject();
            string result;

            try
            {
                var fileName = string.Concat(Server.MapPath(fileSavedPath), "/", savedFileName);

                var importts = new List<ImportTs>();

                var helper = new ImportDataHelper();
                var checkResult = helper.CheckImportData(fileName, importts);

                jo.Add("Result", checkResult.Success);
                jo.Add("Msg", checkResult.Success ? string.Empty : checkResult.ErrorMessage);

                if (checkResult.Success)
                {
                    //儲存匯入的資料
                    helper.SaveImportData(importts);
                }
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Content(result, "application/json");
        }

    }
}