using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AudioApplication.Startup))]
namespace AudioApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
