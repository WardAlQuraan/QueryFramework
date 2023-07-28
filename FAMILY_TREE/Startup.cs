using BASES.BASE_SERVICE;
using COMMON;
using COMMON.APP_SETTINGS;
using CONNECTION_FACTORY.DAL_SESSION;
using CONNECTION_FACTORY.DB_CONNECTION_FACTORY;
using FAMILY_TREE.EXTENSIONS;
using FAMILY_TREE.MIDDLEWARES;
using FAMILY_TREE.MIDDLEWARES.EXCEPTION;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SERVICES.AUTH;
using SERVICES.FAMILY;
using SERVICES.FAMILY.FAMILY_MEMBER;
using SERVICES.ROLE;
using SERVICES.TEST_ENTITY;
using System.Text;

namespace FAMILY_TREE
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public readonly AppSettings _appSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //this._appSettings = appsettings.Value;
        }

        public void ConfigureServices(IServiceCollection services )
        {
            
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // You can add other configuration providers here
            .Build();

            // Configure the options and bind them to the configuration
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Set up authorization for Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = GlobalApp.AppSettings.JWT.Issuer,
                    ValidAudience = GlobalApp.AppSettings.JWT.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalApp.AppSettings.JWT.SecretKey))
                };

            });

            services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDalSession, DalSession>();

            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IFamilyMemberService, FamilyMemberService>();
            
        }
    

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                });

            }
            app.UseMiddleware<AppsettingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();

            app.UseIExceptionHandler("/error");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
