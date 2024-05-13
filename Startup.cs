﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using System.IO;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MyCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; } //proprieta per permettere all avvio del applicazione di leggere il file di configurazione appsettings.json

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; //dipendenza debole
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //registrazione del servizio di dependency a injection:
            // services.AddTransient<ICourseService, CourseService>(); 
            //services.AddTransient<ICourseService, AdoNetCourseService>(); // il servizio precedente (riga 28) che implementava il servizio in maniera random viene sostituito dal nuovo servizio che leggera i dati da db
             services.AddTransient<ICourseService, EfCoreCourseService>();

            // services.AddTransient<ICourseService, AdoNetCourseService>();

            //quando un controller ha nel suo costruttore un oggetto di tipo ICourseService, crea un oggetto di tipo CourseService
            services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
           // services.AddDbContext<MyCourseDbContext>();//registro il servizio DbContext per poter utilizzare Entity Framework Core nell'app
            
           

            services.AddDbContextPool<MyCourseDbContext>(optionsBuilder =>
            {
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");//questo metodo mi permette di recuperare dal file di configurazione appsettings.json il valore di default della sezione ConnectionStrings 
                //string connectionString = Configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlite(connectionString);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //aggiorniamo il file per notificare al Browser Sync che deve aggiornare la pagina
                lifetime.ApplicationStarted.Register(() =>
                {
                    string filePath = Path.Combine(env.ContentRootPath, "bin/reload.txt");
                    File.WriteAllText(filePath, DateTime.Now.ToString());

                });
            }

            //app.UseLiveReload();
            app.UseStaticFiles(); //css, immagini e altri file statici contenuti nella cartella wwwroot
            app.UseRouting();
            app.UseEndpoints(routeBuilder => {
                routeBuilder.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvcWithDefaultRoute(); //IMPOSTA DI DEFAULT controller = Home / action = Index
            //se vuoi modificare la Route puoi usare il metodo app.MapRoute
        }
    }
}