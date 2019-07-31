using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TSMbank.Startup))]
namespace TSMbank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
