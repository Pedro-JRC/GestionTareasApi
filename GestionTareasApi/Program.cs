using GestionTareasApi._3_Servicios;
using GestionTareasApi.Data;
using GestionTareasApi.Eventos;
using GestionTareasApi.Servicios;
using GestionTareasApi.SignalR;
using GestorTareasApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace GestionTareasApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region CONFIGURAR DB CONTEXT

            // CONFIGURA EL CONTEXTO DE BASE DE DATOS CON SQL SERVER
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionPorDefecto")));

            #endregion

            #region REGISTRAR SERVICIOS

            // REGISTRA LOS SERVICIOS PERSONALIZADOS
            builder.Services.AddScoped<TareasService>();
            builder.Services.AddScoped<UsuariosService>();
            builder.Services.AddScoped<ServicioAutenticacion>();
            builder.Services.AddSingleton<ColaTareasRxService>();
            builder.Services.AddSingleton<MemorizadorTareasService>();

            // REGISTRAR EL HUB DE SIGNALR
            builder.Services.AddSignalR();


            #endregion

            #region CONFIGURAR AUTENTICACIÓN JWT

            // OBTIENE LA CLAVE SECRETA DESDE LA CONFIGURACIÓN
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("La clave JWT (Jwt:Key) no está definida en appsettings.json.");

            // CONFIGURA LOS PARÁMETROS DE VALIDACIÓN DEL TOKEN JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            #endregion

            #region CONFIGURAR CONTROLADORES Y SWAGGER

            // AGREGA CONTROLADORES
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // MUESTRA LOS ENUMS COMO STRINGS EN LUGAR DE NÚMEROS
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // CONFIGURA SWAGGER PARA DOCUMENTACIÓN DE LA API
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API de Gestión de Tareas",
                    Version = "v1"
                });

                // CONFIGURA EL ESQUEMA DE SEGURIDAD JWT PARA SWAGGER
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Description = "Ingrese su token JWT en el campo",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            #endregion

            #region CONFIGURAR LOGGER PARA EVENTOS

            // OBTIENE EL LOGGER DEL SISTEMA Y LO CONFIGURA PARA USAR EN EVENTOS DE TAREA
            var app = builder.Build();

            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("EventosTarea");

            EventosTarea.ConfigurarLogger(logger);

            #endregion

            #region MIDDLEWARE GLOBAL

            // AGREGA EL MIDDLEWARE GLOBAL DE MANEJO DE EXCEPCIONES
            app.UseMiddleware<ManejadorExcepcionesMiddleware>();

            #endregion

            #region CONFIGURACIÓN DE SWAGGER

            // MUESTRA SWAGGER SOLO EN ENTORNO DE DESARROLLO
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gestión de Tareas v1");
                    c.RoutePrefix = "swagger";
                });
            }

            #endregion

            #region PIPELINE HTTP

            // CONFIGURA REDIRECCIÓN HTTPS Y AUTENTICACIÓN
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // MAPEA LOS CONTROLADORES
            app.MapControllers();

            // CONFIGURAR LA RUTA DEL HUB DE SIGNALR
            app.MapHub<AppHub>("/hub/app");


            #endregion

            // INICIA LA APLICACIÓN
            app.Run();
        }
    }
}
