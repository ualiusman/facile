using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HandoverTracker.Startup))]
namespace HandoverTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
