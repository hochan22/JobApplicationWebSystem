using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApplicationSystem.Startup))]
namespace ApplicationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
