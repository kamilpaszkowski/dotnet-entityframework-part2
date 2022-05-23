using Euvic.StaffTraining.Common;
using Euvic.StaffTraining.Identity;
using Euvic.StaffTraining.Identity.Infrastructure.EntityFramework;
using Euvic.StaffTraining.WebAPI.Filters;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Euvic.StaffTraining.WebAPI
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
            services.AddDbContext<StaffTrainingContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("Sql"),
               config => config.MigrationsHistoryTable(HistoryRepository.DefaultTableName, StaffTrainingContext.Schema)));

            services.AddDbContext<StaffTrainingReadonlyContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("Sql"),
               config => config.MigrationsHistoryTable(HistoryRepository.DefaultTableName, StaffTrainingReadonlyContext.Schema)));

            services.AddIdentity(Configuration.GetConnectionString("Sql"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Euvic.StaffTraining.WebAPI", Version = "v1" });
            });

            services.AddMvcCore(conf =>
            {
                conf.Filters.Add<ExceptionHandlerFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Euvic.StaffTraining.WebAPI v1"));
                app.Migrate<StaffTrainingContext>();
                app.Migrate<StaffTrainingReadonlyContext>();
                app.Migrate<IdentityContext>();
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
