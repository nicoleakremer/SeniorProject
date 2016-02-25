using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Photography.Startup))]
namespace Photography
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
