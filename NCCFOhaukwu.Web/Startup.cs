using Microsoft.Owin;
using NCCFOhaukwu.Web;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace NCCFOhaukwu.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}