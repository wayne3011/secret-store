using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SecretStore.Web.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SecretStore.Web.Swagger;

public class AuthenticationSwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var allowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
        var adminAuth = context.MethodInfo.GetCustomAttributes(true).OfType<AdminAuthAttribute>().Any();
        if (operation.Security == null)
        {
            operation.Security = new List<OpenApiSecurityRequirement>();
        }
        if (!allowAnonymous && !adminAuth)
        {
            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
            
;
        }
        else if (adminAuth)
        {
            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Api-Key",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Api-Key",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        }

    }
}