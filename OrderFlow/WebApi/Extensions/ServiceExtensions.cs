using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using WebApi.Services;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();
                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(string.Format(@"{0}\OrderFlow.WebAPI.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Clean Architecture - WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "Onos",
                        Email = "marioonosnorbert@gmail.com",
                        Url = new Uri("https://gmail.com"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this WebApi",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default WebApi Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the WebApi version in the request, use the default WebApi version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the WebApi versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });

            services.AddApiVersioning().AddApiExplorer(
               options =>
               {
                   // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                   // note: the specified format code will format the version as "'v'major[.minor][-status]"
                   options.GroupNameFormat = "'v'VVV";

                   // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                   // can also be used to control the format of the WebApi version in route templates
                   options.SubstituteApiVersionInUrl = true;
               });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        public static void AddURIExtension(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
        }
    }

}
