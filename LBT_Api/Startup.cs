using LBT_Api.Entities;
using LBT_Api.Interfaces.Services;
using LBT_Api.Middleware;
using LBT_Api.Services;
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
            // Use Database
            ConfigureDataBaseConections(services);

            // Use HTTP Client
            services.AddHttpClient();

            // Controllers
            services.AddControllers();

            // Automapper
            services.AddAutoMapper(GetType().Assembly);

            // Middleware
            services.AddScoped<ErrorHandlingMiddleware>();

            // Services
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IContactInfoService, ContactInfoService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductSoldService,  ProductSoldService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IWorkerService, WorkerService>();

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
            // CORS
            app.UseCors("Dev");

            // Api documentation
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LBT_Api v1"));

            // Middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Jwt Tokens
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
