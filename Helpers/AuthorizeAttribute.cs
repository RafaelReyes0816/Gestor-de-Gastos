using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gestor_Gastos.Helpers
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public AuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (_allowedRoles != null && _allowedRoles.Length > 0)
            {
                var rol = context.HttpContext.Session.GetString("Rol");
                if (rol == null || !_allowedRoles.Contains(rol))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}

