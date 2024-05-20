using System.Reflection;
using System.Text;using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecretStore.DataAccess.Context;
using SecretStore.Domain.Services.Options;
using SecretStore.Extensions;
using SecretStore.Web.Middlewares;
using SecretStore.Web.Options;
using SecretStore.Web.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreDbContext>(
    opt => 
        opt.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql")));
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "You api title", Version = "v1" });
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme() { In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT", 
            Name = "Authorization", Type = SecuritySchemeType.ApiKey });
    c.AddSecurityDefinition("Api-Key",
        new OpenApiSecurityScheme() { In = ParameterLocation.Header,
            Name = "X-Api-Key",
            Description = "Please enter admin api key", 
            Type = SecuritySchemeType.ApiKey });
    c.OperationFilter<AuthenticationSwaggerFilter>();

});
builder.Services.AddOptions<AdminAccessConfiguration>().BindConfiguration(AdminAccessConfiguration.SectionName);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOptions<JwtConfiguration>().BindConfiguration(JwtConfiguration.SectionName);
builder.Services.AddAuthorization();
builder.WebHost.UseUrls(builder.Configuration["Url"],
    builder.Configuration["AdminAccessConfiguration:AdminAccessUrl"]);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseMiddleware<AuthMiddleware>();
app.MapControllers();
await app.MigrateDatabase();

app.Run();