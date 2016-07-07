using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcApplication1.Startup))]
namespace MvcApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
