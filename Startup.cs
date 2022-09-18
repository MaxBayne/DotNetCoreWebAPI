using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DotNetCoreWebAPI.MiddleWares;

namespace DotNetCoreWebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Register Service For MVC (Model-View-Controller)
            //services.AddMvc();

            //Register Service For Razor Pages
            //services.AddRazorPages();

            //Register Service For Custom Middleware
            services.AddTransient<CustomMiddleWare>();

            //Register Service For Web Api
            services.AddControllers();

            //Register Service For Swagger Client App For Api
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetCoreWebAPI", Version = "v1" });
            });

           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline by middleWare
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //================================================
            //Config the MiddleWares For Http Request Pipeline
            //================================================


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetCoreWebAPI v1"));
            }


            #region Hints

            //Run Method used To Complete the Execution of Http Pipeline and avoid the reminding middleWares
            //----------------------------------------------------------------------------------------------
            /*
            app.Run(async (httpContext)=>
            {
                await httpContext.Response.WriteAsync("Hello World From Run Method");
            });
            */

            //Use Method used To Insert MiddleWare inside Http Request Pipeline , if execute next then it will go to next middleWare otherwise will be back to previous middleWare
            //-------------------------------------------------------------------------------------------------
            /*
            app.Use(async (httpContext, nextMiddleWare) =>
            {
                await httpContext.Response.WriteAsync("hello from use middleWare 1 \n");

                //Go To Next MiddleWare
                await nextMiddleWare();

                //If any code here then will be Executed after back from next MiddleWare
            });

            app.Use(async (httpContext, nextMiddleWare) =>
            {
                await httpContext.Response.WriteAsync("hello from use middleWare 2 \n");

                //Go To Next MiddleWare
                await nextMiddleWare();
            });
            */

            //Map Method used to execute code depend on request url
            //-----------------------------------------------------
            /*
            app.Map("/testmap", (appBuilder) =>
            {
                appBuilder.Use(async (httpContext,nextMiddleWare) =>
                {
                    await httpContext.Response.WriteAsync("Hello From branched middleWare \n");

                    //Go To Next MiddleWare
                    await nextMiddleWare();
                });

            });
            */

            //Custom MiddleWare
            //---------------------------------------------------
            /*
            app.Use(async (httpContext, nextMiddleWare) =>
            {
                await httpContext.Response.WriteAsync("hello from use middleWare 2 \n");

                //Go To Next MiddleWare
                await nextMiddleWare();
            });

            //Use Custom Middleware
            app.UseMiddleware<CustomMiddleWare>();

            app.Run(async (httpContext) =>
            {
                await httpContext.Response.WriteAsync("Hello From Run Method and Complete the Request");
            });

            */

            #endregion


            //Redirect any Http Call To Https
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Use Controllers Action as Endpoints to webApi
                endpoints.MapControllers();
            });

        }
    }
}
