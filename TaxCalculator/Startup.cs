using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Filters;
using TaxCalculator.Infrastructure.Interfaces.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Services;
using TaxCalculator.Services;

namespace TaxCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => 
            {
                options.Filters.Add<DuplicateRequestKeyExceptionFilter>();
            });
            services.AddMemoryCache();
            services.AddTransient<ITaxConfigurationProvider, TaxConfigurationProvider>();
            services.AddTransient<IIncomeTaxCalculator, IncomeTaxCalculator>();
            services.AddTransient<ISocialContributionCalculator, SocialContributionCalculator>();
            services.AddScoped<IIdempotencyService, IdempotencyCachingService>();
            services.AddScoped<IIncomeTaxService, PersonalTaxCalculatorService>();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Calculator V1");
            });
        }
    }
}
