using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(appraisal.Startup))]
namespace appraisal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
