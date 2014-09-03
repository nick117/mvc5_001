using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using appraisal.Models;
using System.Collections;

namespace appraisal.Filters
{
    public class AuthorizeADAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
 //           if (base.AuthorizeCore(httpContext))
   //         {
                /* Return true immediately if the authorization is not 
                locked down to any particular AD group */
                if (String.IsNullOrEmpty(Groups))
                    return true;

                // Get the AD groups
                var groups = Groups.Split(',').ToList<string>();

                // Verify that the user is in the given AD group (if any)
//                var context = new PrincipalContext(ContextType.Domain, "Adimmune");
//                var userPrincipal = UserPrincipal.FindByIdentity(context,
//                                                     IdentityType.SamAccountName,
//                                                     httpContext.User.Identity.Name);
//                var UGS = userPrincipal.GetAuthorizationGroups();
                foreach (var group in groups)
//                    foreach (var ug in UGS)
 //                   if (userPrincipal.IsMemberOf(context, IdentityType.Name, group))
//                        if (ug.Name == group)
                    if (!String.IsNullOrEmpty(SessionHelper.UserGroup))
                            if (SessionHelper.UserGroup.Contains(group+";"))
                            return true;                    
     //       }
            return false;
        }
    }
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string uid = filterContext.HttpContext.User.Identity.Name.ToString();
            string originController = filterContext.RouteData.Values["controller"].ToString();
            string originAction = filterContext.RouteData.Values["action"].ToString();
            string originArea = String.Empty;
            if (filterContext.RouteData.DataTokens.ContainsKey("area"))
                originArea = filterContext.RouteData.DataTokens["area"].ToString();
            string result = "";
            //考核資料管理
            if (filterContext.ActionParameters.ContainsKey("tsv"))
            {
                tsEditViewModels viewModel = (tsEditViewModels)filterContext.ActionParameters["tsv"];
                result = " 員工: " + viewModel.tsa.emp + " 考核類別: " + viewModel.tsa.exm + " 主管: " + viewModel.tsa.boss;

            };
            if (filterContext.ActionParameters.ContainsKey("ts"))
            {
                ts ts1 = (ts)filterContext.ActionParameters["ts"];
                result = " 員工: " + ts1.emp + " 考核類別: " + ts1.exm + " 主管: " + ts1.boss;

            };
            //員工管理
            if (filterContext.ActionParameters.ContainsKey("empv"))
            {
                empEditViewModels viewModel = (empEditViewModels)filterContext.ActionParameters["empv"];
                result = viewModel.emp1.eid + viewModel.emp1.cname + " 職稱: " + viewModel.emp1.title + " 部門編號: " + viewModel.emp1.dept;

            };
            if (filterContext.ActionParameters.ContainsKey("emp"))
            {
                emp emp1 = (emp)filterContext.ActionParameters["emp"];
                result = emp1.eid + emp1.cname + " 職稱: " + emp1.title + " 部門編號: " +emp1.dept;

            };
            //通用
            if (filterContext.ActionParameters.ContainsKey("id"))
            {
                int id1 = (int)filterContext.ActionParameters["id"];
                result = " 序號: " + id1.ToString();

            };
            if (filterContext.ActionParameters.ContainsKey("sid"))
            {
                string id1 = (string)filterContext.ActionParameters["sid"];
                result = " 代號: " + id1;

            };
            //權限管理
            if (filterContext.ActionParameters.ContainsKey("model"))
            {
                LoginModel lm = (LoginModel)filterContext.ActionParameters["model"];
                uid = lm.UserName;
            };
            //評核類別管理
            if (filterContext.ActionParameters.ContainsKey("exm"))
            {
                exm lm = (exm)filterContext.ActionParameters["exm"];
               result = lm.sn+lm.subject;
            };
            //部門管理
            if (filterContext.ActionParameters.ContainsKey("dep"))
            {
                dep lm = (dep)filterContext.ActionParameters["dep"];
                result = lm.title;
            };
            //評核時間管理
            if (filterContext.ActionParameters.ContainsKey("ots"))
            {
                ots lm = (ots)filterContext.ActionParameters["ots"];
                result = lm.Skey + " : " + lm.Vl;
            };
            //Log的資料
            string uname = String.IsNullOrEmpty(SessionHelper.RealName) ? uid : SessionHelper.RealName;
            actlog logmodel = new actlog()
            {
                App = ControllerName+originController,
                Act = ActionName+originAction,
                Pepo = uname.Equals("無此卡號") ? uid : uname,
                Ext = result,
                Tm = DateTime.Now               
            }; 
            db.actlogs.Add(logmodel);
            db.SaveChanges();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string uid = filterContext.HttpContext.User.Identity.Name.ToString();
            actlog logmodel = new actlog()
            {

                App = ControllerName,
                Act = ActionName,
                Pepo = String.IsNullOrEmpty(SessionHelper.RealName) ? uid : SessionHelper.RealName,
                Ext = "離開網頁",
                Tm = DateTime.Now
            };
//            db.actlogs.Add(logmodel);
//            db.SaveChanges();
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string uid = filterContext.HttpContext.User.Identity.Name.ToString();
            actlog logmodel = new actlog()
            {

                App = ControllerName,
                Act = ActionName,
                Pepo = String.IsNullOrEmpty(SessionHelper.RealName) ? uid : SessionHelper.RealName,
                Ext = "傳回的動作結果執行開始",
                Tm = DateTime.Now
            };
   //         db.actlogs.Add(logmodel);
  //          db.SaveChanges();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string uid = filterContext.HttpContext.User.Identity.Name.ToString();
            actlog logmodel = new actlog()
            {

                App = ControllerName,
                Act = ActionName,
                Pepo = String.IsNullOrEmpty(SessionHelper.RealName) ? uid : SessionHelper.RealName,
                Ext = "傳回的動作結果執行完成",
                Tm = DateTime.Now
            };
//            db.actlogs.Add(logmodel);
//            db.SaveChanges();
        }
    }
}