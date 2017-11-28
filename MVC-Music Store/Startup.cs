using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Music_Store.Startup))]
namespace MVC_Music_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
