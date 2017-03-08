using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAPI4.Startup))]
namespace WebAPI4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
