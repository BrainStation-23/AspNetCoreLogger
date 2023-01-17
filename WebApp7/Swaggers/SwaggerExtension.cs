using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace WebApp7.Swaggers
{
    public static class SwaggerExtension
    {
        public static void SwaggerGenConfiguration(this SwaggerGenOptions options)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "WebApp.xml");
            //options.IncludeXmlComments(filePath);
            options.EnableAnnotations();

            options.ExampleFilters();
            

            #region bearer
            //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    In = ParameterLocation.Header,
            //    Description = "Please insert JWT with Bearer into field",
            //    Name = "Authorization",
            //    Type = SecuritySchemeType.ApiKey
            //});
            //options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //   {
            //     new OpenApiSecurityScheme
            //     {
            //       Reference = new OpenApiReference
            //       {
            //         Type = ReferenceType.SecurityScheme,
            //         Id = "Bearer"
            //       }
            //      },
            //      new string[] { }
            //    }
            //  });
            #endregion

            #region form
            //options.DocumentFilter<SwaggerAuthorizationFilter>(); //hide authorize endpoints
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/api/token/generate", UriKind.Relative),

                    }
                },
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { "Bearer"}
                }
            });
            #endregion
        }
    }
}
