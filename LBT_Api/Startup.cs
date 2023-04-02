using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            //Release
            var localDbConectionString = Configuration["DB_CONNECTION_STRING"];
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? localDbConectionString;

            //services.AddDbContext<DbContext>(options =>
            //    options.UseNpgsql(connectionString)
            //);
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

            //if (env.IsDevelopment())
            // {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TypicalSchoolWebsite_API v1"));
            //}

            // Use Middleware
            //app.UseMiddleware<ErrorHandlingMiddleware>();

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
