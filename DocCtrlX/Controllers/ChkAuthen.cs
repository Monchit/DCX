using System.Web;
using System.Web.Mvc;

namespace DocCtrlX.Controllers
{
    public class Chk_Authen : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["DX_Authen"] == null)
            {
                string loginpath = "~/Home/Index";
                if (HttpContext.Current.Request.Url != null)
                {
                    HttpContext.Current.Session["DX_Redirect"] = HttpContext.Current.Request.Url;
                }
                filterContext.Result = new RedirectResult(loginpath);
            }
        }
    }
}