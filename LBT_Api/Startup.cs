using LBT_Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LBT_Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureDataBaseConections(IServiceCollection services)
        {
            var localDbConectionString = Configuration.GetValue<string>("DefaultValues:DbConnectionString");
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? localDbConectionString;

            services.AddDbContext<LBT_DbContext>(options =>
                options.UseNpgsql(connectionString)
            );
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Use Database
            ConfigureDataBaseConections(services);

            //Use HTTP Client
            services.AddHttpClient();

            // Controllers
            services.AddControllers();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LBT_Api", Version = "v1" });
            });

            // CORS
            services.AddCors(o => o.AddPolicy("Dev", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }


        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            //UseCORS
            app.UseCors("Dev");

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LBT_Api v1"));

            // Use Jwt Tokens
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
