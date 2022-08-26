using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrabalhoPartico.Startup))]
namespace TrabalhoPartico
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
