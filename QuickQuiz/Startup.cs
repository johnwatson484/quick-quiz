using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuickQuiz.Startup))]
namespace QuickQuiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
