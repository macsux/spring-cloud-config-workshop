using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DeathStar
{
    public class StarWarsConfig
    {
        public string ErrorMessage { get; set; }
        public LightSaber LightSaber { get; set; }
        public string RebelBase { get; set; }
        public string Jedi { get; set; }
        public string Mastery { get; set; }    
    }

    public class LightSaber
    {
        public string Color { get; set; }
    }
    public class Startup
    {
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<StarWarsConfig>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptionsSnapshot<StarWarsConfig> config)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"{nameof(StarWarsConfig.ErrorMessage)}:{config.Value.ErrorMessage}\n");
                await context.Response.WriteAsync($"{nameof(StarWarsConfig.RebelBase)}:{config.Value.RebelBase}\n");
                await context.Response.WriteAsync($"{nameof(StarWarsConfig.LightSaber.Color)}:{config.Value.LightSaber.Color}\n");
                await context.Response.WriteAsync($"{nameof(StarWarsConfig.Mastery)}:{config.Value.Mastery}\n");
//                var configValues = Configuration.AsEnumerable().ToList();
//                foreach (var (key, value) in configValues)
//                {
//                    await context.Response.WriteAsync($"{key}: {value}\n");    
//                }
                
            });
        }
    }
}