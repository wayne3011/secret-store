using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecretStore.Domain.Services.Options;
using SecretStore.Web.Attributes;
using SecretStore.Web.Exceptions;
using SecretStore.Web.Options;

namespace SecretStore.Web.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly AdminAccessConfiguration _adminAccessConfiguration;

    public AuthMiddleware(RequestDelegate next, IOptions<JwtConfiguration> jwtConfiguration, IOptions<AdminAccessConfiguration> adminAccessConfiguration)
    {
        _next = next;
        _adminAccessConfiguration = adminAccessConfiguration.Value;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var allowAnonymous = context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() is not null;
        var adminEndpoint = context.GetEndpoint()?.Metadata.GetMetadata<AdminAuthAttribute>() is not null;
        if (!allowAnonymous && !adminEndpoint)
        {
            AuthenticationHeaderValue.TryParse(context.Request.Headers.Authorization, out var token);
            var options = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _jwtConfiguration.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfiguration.Issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey))
            };
            var handler = new JwtSecurityTokenHandler();
            var result = await handler.ValidateTokenAsync(token?.Parameter, options);

            if (!result.IsValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            else
            {
                var userId = Guid.Parse((string)result.Claims.First(x => x.Key == "UserId").Value);

                context.Items.Add(new KeyValuePair<object, object?>("UserId", userId));   
            }
        }
        else if (adminEndpoint)
        { 
            var adminUrl = new Uri(_adminAccessConfiguration.AdminAccessUrl);
            if (context.Connection.LocalPort != adminUrl.Port)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var haveHeader = context.Request.Headers.TryGetValue(_adminAccessConfiguration.AdminAuthHeader, out var header);
            if (!haveHeader || header != _adminAccessConfiguration.AdminApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        await _next(context);   
    }
}
