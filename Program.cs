using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaGestion.Helpers;
using SistemaGestion.Interfaces;
using SistemaGestion.Models;
using SistemaGestion.Servicios;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<sistemaGestionBD>(options => options.UseSqlServer(builder.Configuration.GetConnectionString ("Development")));
builder.Services.AddTransient<IUsuario, ServiceUsuario>();
builder.Services.AddTransient<IEmpresa, ServiceEmpresa>();
builder.Services.AddTransient<IEmpleado, ServiceEmpleado>();
builder.Services.AddTransient<IContacto, ServiceContacto>();
builder.Services.AddTransient<IGeografia, ServiceGeografia>();




builder.Services.AddTransient<ISqlQueryDynamicJson, SqlQueryDynamicJson>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                        (builder.Configuration.GetSection("AppSettings:SecretKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistema Gestion", Version = "v1" });
    c.OperationFilter<SecurityRequirementsOperationFilter>();

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Autorizacion Standar, Usar Bearer. Ej: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(x =>
    x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
