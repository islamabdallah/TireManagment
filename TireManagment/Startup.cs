using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Hubs;
using TireManagment.Services;

namespace TireManagment
{
    public class Startup
    {
        private RoleManager<IdentityRole> manager;
        CategoryService CategoryService;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("CS");
            services.AddDbContext<DbContext>(c =>
            c.UseSqlServer(connectionString));
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<DbContext>();
            // services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<Context>();
            // services.AddScoped<AccountService>();
            services.AddControllersWithViews();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.AddScoped<AccountService>();
            services.AddScoped<TireService>();
            services.AddScoped<TruckService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<BrandService>();
            services.AddScoped<TruckTireService>();
            services.AddSignalR();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;

                options.Password.RequireNonAlphanumeric = false;


            });
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Handastic", Version = "v1" });
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(RoleManager<IdentityRole> _manager, IApplicationBuilder app, IWebHostEnvironment env, CategoryService _categoryService)
        {

            CategoryService = _categoryService;
            manager = _manager;
            CreateRoles().Wait();
            insertdata();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Test.Web.Api");
                //    c.RoutePrefix = string.Empty;
                //});
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

          

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<notifyHub>("/questionHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        public async Task CreateRoles()
        {

            if (!await manager.RoleExistsAsync(Role.Admin))
                await manager.CreateAsync(new IdentityRole(Role.Admin));
            if (!await manager.RoleExistsAsync(Role.TireMan))
                await manager.CreateAsync(new IdentityRole(Role.TireMan));

        }
        public void insertdata()
        {
            if (CategoryService.GetAll().ToList().Count == 0)
            {
                List<TruckCategory> categories = new List<TruckCategory> {
            new TruckCategory(){ Category="truck 22 tire", TireCount=23, RareTires=12,FrontTires=10,SpareTires=1, Image="22 Tire.png"},
             new TruckCategory(){Category="truck 14 tire", TireCount=15, RareTires=10,FrontTires=4,SpareTires=1,Image="14 Tire.png"},
              new TruckCategory(){Category="truck 12 tire", TireCount=13, RareTires=8,FrontTires=4,SpareTires=1,Image="12Tires.png"},
               new TruckCategory(){Category="truck 10 tire", TireCount=11, RareTires=8,FrontTires=2,SpareTires=1,Image="10 Tires.png"},
                new TruckCategory(){Category="truck 4 tire", TireCount=5, RareTires=2,FrontTires=2,SpareTires=1,Image="4 Tires.png"}

                };
                CategoryService.InsertList(categories);

            }
        }
    }
}

