using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Data;
using Microsoft.EntityFrameworkCore;

namespace MyTelegramBot
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
         
            services.AddDbContext<ProductContext>(options =>
            
                options.UseSqlite("Data Source=ProductDatabase.db")//Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Setting Telegram
            services.AddTransient<ITelegramApiRequest, TelegramApiRequest>();
            services.Configure<TelegramSettings>(Configuration.GetSection("TelegramSettings"));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IReceiver, FileReceiver>();
            services.AddSingleton<IMyLogger, MyLogger>();

         /*    services.AddRouting(options => {
                options.ConstraintMap["inputMessage"] = typeof(MessageForCreationDto);
                options.ConstraintMap["inputCallbackQuery"] = typeof(CallbackQuery);
                //options.ConstraintMap.Add("inputMessage", typeof(MessageForCreationDto));
            }); */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
