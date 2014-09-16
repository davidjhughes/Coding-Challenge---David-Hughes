using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PonyMusic.Startup))]
namespace PonyMusic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
