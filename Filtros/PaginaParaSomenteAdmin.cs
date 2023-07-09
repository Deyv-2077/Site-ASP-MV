using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MVCApp2.Models;
using Newtonsoft.Json;


namespace MVCApp2.Filtros
{
    public class PaginaParaSomenteAdmin : ActionFilterAttribute
    {
            public override void OnActionExecuted(ActionExecutedContext context)
            {
                string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");

                if (string.IsNullOrEmpty(sessaoUsuario))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Home1" } });
                }
                else
                {
                    UserModel2 userModel2 = JsonConvert.DeserializeObject<UserModel2>(sessaoUsuario);
                  
                if (userModel2 == null)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Home1" } });

                    }
                if (userModel2.Perfil != Enums.PerfilEnum.Admin)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Restrito" }, { "action", "Home1" } });

                }
            
                base.OnActionExecuted(context);
            }


        }
    }
}
