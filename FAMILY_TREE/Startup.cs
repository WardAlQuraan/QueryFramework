using BASES.BASE_SERVICE;
using COMMON;
using COMMON.APP_SETTINGS;
using CONNECTION_FACTORY.DAL_SESSION;
using CONNECTION_FACTORY.DB_CONNECTION_FACTORY;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SERVICES.TEST_ENTITY;

namespace FAMILY_TREE
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            GlobalApp.AppSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddSingleton<IDbConnectionFactory,DbConnectionFactory>();
            services.AddSingleton<IDalSession,DalSession>();

            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<ITestEntityService , TestEntityService>();

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

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
