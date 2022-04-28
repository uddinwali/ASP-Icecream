using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TailSpyWorksAdvanced.Startup))]
namespace TailSpyWorksAdvanced
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
