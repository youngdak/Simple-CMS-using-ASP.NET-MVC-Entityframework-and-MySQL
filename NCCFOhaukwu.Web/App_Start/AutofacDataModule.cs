using Autofac;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web
{
    public class AutofacDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Service>().As<IService>().InstancePerRequest();
            base.Load(builder);
        }
    }
}