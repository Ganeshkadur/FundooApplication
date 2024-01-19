using BusenessLayer.Interfaces;
using BusenessLayer.Service;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FandooNotesApp
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
            services.AddControllers();

            #region swagger
            services.AddSwaggerGen();
            #endregion

            #region dbconnection
            services.AddDbContext<Context>(a => a.UseSqlServer(Configuration["ConnectionStrings:Connectiondb"]));
            #endregion

            #region Encrypt/decript the data
            services.AddDataProtection();
            #endregion

            #region dependency injection
            services.AddScoped<IFandoo_Buseness, Fandoo_Buseness>();
            services.AddScoped<IFandoo_Repository, Fandoo_Repository>();

            services.AddScoped<INotes_Buseness, Notes_Buseness>();
            services.AddScoped<INotes_Repository, Notes_Repository>();

            services.AddScoped<ICollaborator_Repository, Collaborator_Repository>();
            services.AddScoped<ICollaborator_Buseness, Collaborator_Buseness>();

            services.AddScoped<ILabel_Buseness, Label_Buseness>();
            services.AddScoped<ILabel_Repository, Label_Repository>();
            #endregion

            #region jwt
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };


            });



            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });


                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Jwt",
                    Scheme = "Bearer"
                });


                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                 {
                     new OpenApiSecurityScheme
                             {
                   Reference = new OpenApiReference
                        {
                     Type=ReferenceType.SecurityScheme,
                         Id="Bearer"
                         }
                          },
                      new string[]{}
                     }
                     });
            });
            #endregion

            #region rabbit mq
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddScoped<IBus>(provider => provider.GetRequiredService<IBusControl>());
            #endregion

            #region redis cache

            services.AddDistributedRedisCache(
                options =>
                {
                    options.Configuration = "localhost:6379";
                }
                );
            #endregion

            #region session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });
            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();//session

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();// jwt

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });



        }
    }
}
