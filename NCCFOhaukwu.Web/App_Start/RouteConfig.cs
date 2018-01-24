using System.Web.Mvc;
using System.Web.Routing;
using LowercaseDashedRouting;

namespace NCCFOhaukwu.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.Add(new LowercaseDashedRoute("{controller}/{action}/{id}",
                new RouteValueDictionary(
                    new {controller = "Home", action = "Index", id = UrlParameter.Optional}),
                new DashedRouteHandler()
                ));
        }
    }
}