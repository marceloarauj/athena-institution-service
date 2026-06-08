using AthenaUnionLibrary.Authorization;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

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
                services.AddScoped<IDayLessonRepository, DayLessonRepository>();
                services.AddScoped<IStudentDayLessonRepository, StudentDayLessonRepository>();
                services.AddScoped<IDayLessonScheduleConfigRepository, DayLessonScheduleConfigRepository>();
                services.AddScoped<IDayLessonScheduleConfigHistoryRepository, DayLessonScheduleConfigHistoryRepository>();

                services.AddScoped<IAcademicProgramRepository, AcademicProgramRepository>();
                services.AddScoped<ISubjectRepository, SubjectRepository>();
                services.AddScoped<IRoomRepository, RoomRepository>();
                services.AddScoped<IShiftRepository, ShiftRepository>();
                services.AddScoped<ITeacherRepository, TeacherRepository>();
                services.AddScoped<IProgramEditionRepository, ProgramEditionRepository>();
                services.AddScoped<ICurriculumEntryRepository, CurriculumEntryRepository>();
                services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
                services.AddScoped<IHolidayRepository, HolidayRepository>();
                services.AddScoped<IProgramPeriodRepository, ProgramPeriodRepository>();
                services.AddScoped<ICalendarDayRepository, CalendarDayRepository>();
                services.AddScoped<IProgressRecordRepository, ProgressRecordRepository>();
                services.AddScoped<IClassGroupRepository, ClassGroupRepository>();
                services.AddScoped<IClassScheduleRepository, ClassScheduleRepository>();
                services.AddScoped<IConflictReportRepository, ConflictReportRepository>();

                return services;
            }

            public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
            {
                var key = Encoding.ASCII.GetBytes(configuration["Identity:Key"]!);

                services.AddAthenaAuthorization();

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidIssuer = configuration["Identity:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["Identity:Audience"],
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnForbidden = async ctx =>
                            {
                                ctx.Response.StatusCode = 403;
                                ctx.Response.ContentType = "application/json";
                                var body = new { success = false, message = "You don't have permission to perform this action.", data = (object?)null, errors = (object?)null };
                                await ctx.Response.WriteAsJsonAsync(body);
                            }
                        };
                    });

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
