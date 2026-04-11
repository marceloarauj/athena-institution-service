using Institution.Infrastructure.Contexts.Middlewares;

namespace Institution.Infrastructure
{
    public static class WebApplicationExtensions
    {
        extension(WebApplication app)
        {
            public void UseMiddlewares()
            {
                app.UseMiddleware<InstitutionMiddleware>();
            }
        }
    }
}