using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rosterd.Web.Infra.Security
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires role-based authorization. <br />
    /// To authorize users with either role A or role B, use:
    /// <code>
    /// [AuthorizeByRole("A", "B")]
    /// </code>
    /// To only authorize users with both role A and role B, use:
    /// <code>
    /// [AuthorizeByRole("A")] <br />
    /// [AuthorizeByRole("B")]
    /// </code>
    /// </summary>
    public class AuthorizeByRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeByRoleAttribute(params string[] roles) => Roles = string.Join(",", roles);
    }
}
