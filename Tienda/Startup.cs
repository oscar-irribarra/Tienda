using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tienda.Startup))]
namespace Tienda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
