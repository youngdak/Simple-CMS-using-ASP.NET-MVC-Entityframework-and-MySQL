using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NCCFOhaukwu.Web
{
    public class MvcApplication : HttpApplication
    {
        //private readonly EntityConnectionStringBuilder ConnString =
        //    new EntityConnectionStringBuilder(
        //        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.ConfigureContainer();

            //var connectionString = ConnString.ProviderConnectionString;
            //SqlDependency.Start(connectionString);
        }

        protected void Application_End()
        {
            //var connectionString = ConnString.ProviderConnectionString;
            //SqlDependency.Stop(connectionString);
        }
    }
}