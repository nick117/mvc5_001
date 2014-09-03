using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using FluentSecurity.Specification.Policy.ViolationHandlers;
using FluentSecurity;
using appraisal.Controllers;

namespace appraisal.Utilities.Helper
{
    public class Security
    {
        public static ISecurityConfiguration SetFluentSecurity()
        {
            SecurityConfigurator.Configure(configue =>
            {
                //設定去那裡取得關於驗證訊息
                configue.GetAuthenticationStatusFrom(IsUserAuthenticated);
                configue.GetRolesFrom(UserRoles);

                //所有頁面都可以瀏覽
                configue.ForAllControllers().Ignore();

                //Home/About 頁面一定要登入了才能進入
                //configue.For<HomeController>(hc => hc.About()).DenyAnonymousAccess();
                //Home/Contact頁面一定要Role是User才能進入
                //configue.For<HomeController>(hc => hc.Contact()).RequireAnyRole("Everyone");

                configue.For<HomeController>().DenyAnonymousAccess();

                //如果不符合Policy，預設傳回UnAuthorized。換句話說，重新導向登入頁面
                //如果沒有加，預設是直接丟出exception
                configue.DefaultPolicyViolationHandlerIs(() => new HttpUnauthorizedPolicyViolationHandler());

                configue.ForAllControllersInNamespaceContainingType<appraisal.Areas.Admin.Controllers.AdminHomeController>()
                    .AddPolicy<AdminPolicy>();

                //如果沒有加上這一行會鬼打牆，因為上面那一個說所有Area.Admin.Controllers都需要符合
                //AdminPolicy，而下面又說如果不符合轉向到Account/Index，如果Account/Index不允許任何人
                //進入，那麼就會進入無限回圈。
                configue.For<appraisal.Areas.Admin.Controllers.AdminAccountController>()
                    .Ignore();

                configue.Advanced.Violations(violation =>
                {
                    violation.Of<AdminPolicy>().IsHandledBy(() => new AdminPolicyViolationHandler());
                });
            });


            return SecurityConfiguration.Current;
        }

        public static bool IsUserAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        
        //在Form Auth下, 無法使用AD的Roles, 所以下面改成這樣也沒屁用
        public static IEnumerable<object> UserRoles()
        {
            var currentUser = HttpContext.Current.User;
            var userGroup = string.IsNullOrEmpty(currentUser.Identity.Name) ? null : Groups();
           return userGroup as IEnumerable<object>;
        }

       //抓Coneection String 的 Logon Account的群組, 真的沒屁用
        public static ArrayList Groups()
        {
            ArrayList groups = new ArrayList();
            foreach (System.Security.Principal.IdentityReference group in
                System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                groups.Add(group.Translate(typeof
                    (System.Security.Principal.NTAccount)).ToString());
            }
            return groups;
        }
    }
}