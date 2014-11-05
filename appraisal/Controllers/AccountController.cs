using System.Web.Mvc;
using System.Web.Security;
//using System.Web.Hosting;
using System.Linq;
using appraisal.Models;
using appraisal.Filters;
using System.DirectoryServices.AccountManagement;

public class AccountController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();
    public ActionResult Login()
    {
        return this.View();
    }

    [LogActionFilter(ControllerName = "權限管理", ActionName = "開始登入")]
    [HttpPost]
    public ActionResult Login(LoginModel model, string returnUrl)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (Membership.ValidateUser(model.UserName, model.Password))
        {
            //取回登入者姓名塞入Session
            var emp = db.emps;
            var items = from item in emp select item;
            items = items.Where(s => s.eid.ToUpper().Equals(model.UserName.ToUpper()));
            if (items.ToList().Count == 0)
            { SessionHelper.RealName = "無此卡號"; }
            else
            { SessionHelper.RealName = items.ToList().First().cname; }
            //取回AD Group資料塞入Session
            string uGroup = "";
            string webadmin = "K1336;K0948;K0965;K1116;K1433;";
            if (webadmin.Contains(model.UserName.ToUpper()))
            {
                uGroup += "webAdmin01;";
            };
            string webhr = "K1336;K0948;K0965;K1116;K1433;";
            if (webhr.Contains(model.UserName.ToUpper()))
            {
                uGroup +=  "webHr01;";
            };
 //           using (HostingEnvironment.Impersonate())
 //           {
 //               var context = new PrincipalContext(ContextType.Domain, "adimmune.com.tw");
 //               var userPrincipal = UserPrincipal.FindByIdentity(context,IdentityType.SamAccountName,model.UserName);
 //               var UGS = userPrincipal.GetAuthorizationGroups();
 //               foreach (var ug in UGS)
 //                   uGroup = uGroup + ug.Name + ";";
 //           }
            SessionHelper.UserGroup = uGroup;
            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        this.ModelState.AddModelError(string.Empty, "您提供的Windows帳號或密碼並不正確,如有疑問請與資訊人員聯絡.");

        return this.View(model);
    }

    [LogActionFilter(ControllerName = "權限管理", ActionName = "已登出")]
    public ActionResult LogOff()
    {
        SessionHelper.UserGroup = "";
        SessionHelper.RealName = "";
        FormsAuthentication.SignOut();

        return this.RedirectToAction("Index", "Home");
    }
}