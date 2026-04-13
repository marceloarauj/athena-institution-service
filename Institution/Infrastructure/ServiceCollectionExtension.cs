using Institution.Application.Auth;
using Institution.Application.Handlers;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Institution.Application.Interfaces.Services;
using Institution.Application.Options;
using Institution.Infrastructure.Contexts.Models;
using Institution.Infrastructure.Database;
using Institution.Infrastructure.Repositories;
using Institution.Infrastructure.Services;
using Mediator.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddService(IConfiguration configuration)
            {
                services.AddDbContext<AppDbContext>( config =>
                {
                    config.UseNpgsql(configuration.GetConnectionString("postgresql"));
                });

                services.AddHttpContextAccessor();

                services.AddScoped<InstitutionContext>();
                services.AddScoped<IInstitutionContext>(sp => sp.GetRequiredService<InstitutionContext>());

                services.Configure<AmazonS3Options>(configuration.GetSection("Aws"));

                services.AddScoped<IUser, UserAuthentication>();
                services.AddScoped<IInstitutionService, InstitutionService>();
                services.AddScoped<IAmazonService, AmazonService>();
                
                services.AddSingleton<IGradeService, GradeService>();

                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IInstitutionRepository, InstitutionRepository>();
                services.AddScoped<IInstitutionUpdateHistoryRepository, InstitutionUpdateHistoryRepository>();
                services.AddScoped<IEventRepository, EventRepository>();
                services.AddScoped<IDisciplineRepository, DisciplineRepository>();
                services.AddScoped<IDisciplineUpdateHistoryRepository, DisciplineUpdateHistoryRepository>();
                services.AddScoped<IClassroomRepository, ClassroomRepository>();
                services.AddScoped<IStudentClassroomRegistrationRepository, StudentClassroomRegistrationRepository>();

                return services;
            }

            public IServiceCollection AddMediatorConfig()
            {
                services.AddMediator(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(CreateInstitutionHandler).Assembly);
                    cfg.RegisterServicesFromAssembly(typeof(UpdateInstitutionHandler).Assembly);
                    cfg.RegisterServicesFromAssembly(typeof(UpdateEvaluationVariableHandler).Assembly);
                });

                return services;
            }
        }
    }
}
