using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_API.Repositories;
using BATTLESHIP_CORE_API.UnitOfWorks;
using BATTLESHIP_CORE_API.Services;

namespace BATTLESHIP_CORE_API
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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            });


            services.AddDbContext<DBContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));


            services.AddCors(o => o.AddPolicy("CorsPolicy",
               builder =>
               {
                   builder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowAnyOrigin();
               }));

      
            //Repo&Service
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IShipRepository, ShipRepository>();
            services.AddTransient<IActionShipInstallRepository, ActionShipInstallRepository>();
            services.AddTransient<IActionMoveRepository, ActionMoveRepository>();
            services.AddTransient<IGameService, GameService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
