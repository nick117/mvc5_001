using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentSecurity;
using FluentSecurity.Policy;

namespace appraisal.Utilities.Helper
{
    /// <summary>
    /// 自定義Policy，只有Roles是Admin才過
    /// </summary>
    public class AdminPolicy : ISecurityPolicy
    {
        public PolicyResult Enforce(FluentSecurity.ISecurityContext context)
        {

            PolicyResult result = PolicyResult.CreateFailureResult(this, "Access denied!");

            if (context.CurrentUserRoles() != null)
            {
                if (context.CurrentUserRoles().Contains("administrators"))
                {
                    result = PolicyResult.CreateSuccessResult(this);
                }
            }

            return result;
        }
    }
}