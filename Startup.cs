using MyTelegramBot.Interface;
using MyTelegramBot.Web;
using MyTelegramBot.Log;
using MyTelegramBot.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTelegramBot.Data;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Checkers.Messages;
using MyTelegramBot.Checkers.Callback;
using AutoMapper;
using MyTelegramBot.Helpers;

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
         
            services.AddDbContextPool<DataContext>(options =>    
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), x =>
                    x.SuppressForeignKeyEnforcement())
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();

            // Setting Telegram
            var telegramConfig = new TelegramSettings();
            Configuration.Bind("TelegramSettings", telegramConfig);
            services.AddSingleton(telegramConfig);
            services.AddTransient<ITelegramApiRequest, TelegramApiRequest>();  
            //services.Configure<TelegramSettings>(Configuration.GetSection("TelegramSettings");

            services.Configure<FilePaths>(Configuration.GetSection("FilePaths"));
            services.AddScoped<IReceiver, FileReceiver>();
            services.AddScoped<IMyLogger, MyLogger>();

            services.AddTransient<IDataRepository, DataRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            // Setting BD
            services.AddSingleton(provider => new MapperConfiguration(cfg => {
                var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetService<DataContext>();

                cfg.AddProfile(new AutoMapperProfiles(context));
            }).CreateMapper());
            //services.AddAutoMapper(typeof(Startup));
            // Messages
            
            services.AddScoped<DataChecker>();
            services.AddScoped<MainMenuChecker>();
            services.AddScoped<SimpleCommandChecker>();
            services.AddScoped<IMessageChecker>(provider => {
                var _messageChecker = (IMessageChecker)provider.GetService<DataChecker>();
                _messageChecker
                    .SetNext(provider.GetService<MainMenuChecker>())
                    .SetNext(provider.GetService<SimpleCommandChecker>());
                
                return _messageChecker;
            });

            services.AddScoped<DataCallbackChecker>();
            services.AddScoped<CallbackChecker>();
            services.AddScoped<ICallbackChecker>(provider => {
                var _callbackChecker = (ICallbackChecker)provider.GetService<DataCallbackChecker>();
                _callbackChecker
                    .SetNext(provider.GetService<CallbackChecker>());

                return _callbackChecker;
            });

            services.AddTransient<Seed>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app
            //,IServiceProvider provider
            ,Seed seed
            ,IHostingEnvironment env)
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
            seed.SeedProducts();
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