using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Spock_BlogMain.Startup))]
namespace Spock_BlogMain
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
