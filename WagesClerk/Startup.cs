using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WageClerk.Data;
using WageClerk.Data.Models;
using WagesClerk.Data;

namespace WagesClerk
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
            services.AddSwaggerGen();
            services.AddControllers();

            //Register Unit of work
            services
                .AddDbContext<InnovationDayContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]), ServiceLifetime.Transient)
                .AddUnitOfWork<InnovationDayContext>();

            services.AddTransient<IEmployeeShiftDataRepository, EmployeeShiftDataRepository>();
            services.AddTransient<IEmployeeDataRepository, EmployeeDataRepository>();
            services.AddTransient<IShiftDataRepository, ShiftDataRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wages clerk");
            });

            app.UseRouting();
            app.UseAuthorization();
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
