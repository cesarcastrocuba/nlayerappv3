using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLayerApp.Application.MainBoundedContext.BankingModule.Services;
using NLayerApp.Application.MainBoundedContext.ERPModule.Services;
using NLayerApp.DistributedServices.MainBoundedContext;
using NLayerApp.DistributedServices.Seedwork.Filters;
using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
using NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
using NLayerApp.Infrastructure.Crosscutting.Adapter;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
using NLayerApp.Infrastructure.Crosscutting.Validator;
using NLayerApp.Infrastructure.Data.MainBoundedContext.BankingModule.Repositories;
using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
using System.IO;

namespace DistributedServices.MainBoundedContext
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure EntityFramework to use an InMemory database.
            services.AddDbContext<MainBCUnitOfWork>();

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidateModelAttribute()); // an instance
                options.Filters.Add(typeof(LoggerAttribute));
            })
            .AddFluentValidation(fv => { });
            
            //Custom Exception and validation Filter
            services.AddScoped<CustomExceptionFilterAttribute>();

            //Repositories
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            
            // Domain Services
            services.AddScoped<IBankTransferService, BankTransferService>();

            //Services
            services.AddScoped<ISalesAppService, SalesAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IBankAppService, BankAppService>();

            //Adapters
            services.AddScoped<ITypeAdapterFactory, AutomapperTypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(services.BuildServiceProvider().GetService<ITypeAdapterFactory>());

            //Validator
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "DDD N-Layered Architecture",
                    Description = "DDD N-Layered Architecture with .Net Core 2 (NLayerAppV3)",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "César Castro", Email = "cesar_castro_cuba@msn.com", Url = "https://www.linkedin.com/in/c%C3%A9sar-castro-91b56211/" },
                    License = new Swashbuckle.AspNetCore.Swagger.License { Name = "Use under my own License", Url = "https://www.microsoft.com/net/learn/architecture" }
                });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "NLayerApp.DistributedServices.MainBoundedContext.xml");
                c.IncludeXmlComments(xmlPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, MainBCUnitOfWork context1)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            DbInitializer.Initialize(context1);
        }
    }
}
