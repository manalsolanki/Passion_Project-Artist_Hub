using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtistsHub.Startup))]
namespace ArtistsHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
