using Institution.Application.Interfaces;
using Institution.Infrastructure.Contexts.Models;
using Microsoft.AspNetCore.Http;

namespace Institution.Infrastructure.Contexts.Middlewares
{
    public class InstitutionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke
        (
            HttpContext context,
            InstitutionContext institutionContext,
            IUnitOfWork unitOfWork
        )
        {
            //TODO: Get institution alias from request (e.g., from headers, query string, etc.)
            institutionContext.Alias = "instituicao-estadual-maria";
            await _next(context);
        }
    }
}