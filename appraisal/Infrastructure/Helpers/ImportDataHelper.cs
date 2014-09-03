using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using appraisal.Models;
using LinqToExcel;

namespace appraisal.Infrastructure.Helpers
{
    public class ImportDataHelper
    {
        /// <summary>
        /// 檢查匯入的 Excel 資料.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="importTs">The import ts records.</param>
        /// <returns></returns>
        public CheckResult CheckImportData(
            string fileName,
            List<ImportTs> importts)
        {
            var result = new CheckResult();

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "匯入的資料檔案不存在";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //欄位對映
            excelFile.AddMapping<ImportTs>(x => x.CardNo, "卡號");
            excelFile.AddMapping<ImportTs>(x => x.Name, "姓  名");
            excelFile.AddMapping<ImportTs>(x => x.Depart, "部門");
            excelFile.AddMapping<ImportTs>(x => x.Group, "組別");
            excelFile.AddMapping<ImportTs>(x => x.MTitle, "管理職稱");
            excelFile.AddMapping<ImportTs>(x => x.PTitle, "專業職稱");
            excelFile.AddMapping<ImportTs>(x => x.Personal, "個人");
            excelFile.AddMapping<ImportTs>(x => x.Job1, "今年曾任單位1");
            excelFile.AddMapping<ImportTs>(x => x.Job2, "今年曾任單位2");
            excelFile.AddMapping<ImportTs>(x => x.Type, "考核等級");
            excelFile.AddMapping<ImportTs>(x => x.CardNo1, "初評人員卡號");
            excelFile.AddMapping<ImportTs>(x => x.Name1, "初評人員");
            excelFile.AddMapping<ImportTs>(x => x.CardNo2, "複評人員卡號");
            excelFile.AddMapping<ImportTs>(x => x.Name2, "複評人員");

            //SheetName
            var excelContent = excelFile.Worksheet<ImportTs>("名單");

            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            //檢查資料
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var its = new ImportTs();

                //CardNo
                if (string.IsNullOrWhiteSpace(row.CardNo))
                {
                    errorMessage.Append("卡號 - 不可空白. ");
                }
                its.CardNo = row.CardNo;


                //Name
                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errorMessage.Append("姓名 - 不可空白. ");
                }
                its.Name = row.Name;

                its.Depart = row.Depart;
                its.Group = row.Group;
                its.MTitle = row.MTitle;
                its.PTitle = row.PTitle;
                its.Personal = row.Personal;
                its.Job1 = row.Job1;
                its.Job2 = row.Job2;

                //Type
   //             if (string.IsNullOrWhiteSpace(row.Type))
   //             {
   //                 errorMessage.Append("考核等級 - 不可空白. ");
   //             }
                its.Type = row.Type;

                its.CardNo1 = row.CardNo1;
                its.Name1 = row.Name1;
                its.CardNo2 = row.CardNo2;
                its.Name2 = row.Name2;

                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errorCount += 1;
                    importErrorMessages.Add(string.Format(
                        "第 {0} 列資料發現錯誤：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                importts.Add(its);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importts.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Saves the import data.
        /// </summary>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveImportData(IEnumerable<ImportTs> its)
        {
            try
            {
                //先砍掉全部資料
                using ( var db = new ApplicationDbContext())
                {
                    foreach (var item in db.importtss.OrderBy(x => x.CardNo))
                    {
                        db.importtss.Remove(item);
                    }
                    db.SaveChanges();
                }

                //再把匯入的資料給存到資料庫
                using (var db = new ApplicationDbContext())
                {
                    foreach (var item in its)
                    {
                        db.importtss.Add(item);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}