using Microsoft.Owin;
using Owin;
using PostcardsManager;

[assembly: OwinStartup(typeof(Startup))]
namespace PostcardsManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}