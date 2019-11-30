using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace UserApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region JWT
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            services.AddCors(); //Se habilita esta línea para el consumo desde Vue.js, aquí encontré la respuesta: https://code-maze.com/enabling-cors-in-asp-net-core/

            #region Integración para Angular
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            #endregion
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseCors(builder =>
            //            builder.WithOrigins("http://localhost:8080")//Ip del cliente vue, SE AGREGA ESTA LÍNEA. Aquí encontré la respuesta: https://code-maze.com/enabling-cors-in-asp-net-core/
            //                    .WithMethods("DELETE")//Se debe especificar el método para el cual se habilitar el CORS (no mencionar al grupo, en el sentido que investiguen
            //                    );


            /*Para invocar desde Angular se debe comentar la línea anterior(82-85)
             * 
             *  app.UseCors(builder =>
                        builder.WithOrigins("http://localhost:8080")//Ip del cliente vue, SE AGREGA ESTA LÍNEA. Aquí encontré la respuesta: https://code-maze.com/enabling-cors-in-asp-net-core/
                                .WithMethods("DELETE")//Se debe especificar el método para el cual se habilitar el CORS (no mencionar al grupo, en el sentido que investiguen
                                .WithMethods("POST")
                                ); 
             */

            app.UseHttpsRedirection();
            app.UseAuthentication(); //Integración JWT

            app.UseCors("CorsPolicy"); //Integración para Angular
            app.UseMvc();
        }
    }
}
