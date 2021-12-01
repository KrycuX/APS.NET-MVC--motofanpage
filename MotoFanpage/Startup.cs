using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MotoFanpage.Startup))]
namespace MotoFanpage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
