using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;

namespace Calendar.Attributes
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                string redirectUrl = $"/account/external-login?provider=Google&returnUrl={HttpUtility.UrlEncode("/Home/Index")}";
                context.HttpContext.Response.Redirect(redirectUrl);
            }
        }
    }
}
