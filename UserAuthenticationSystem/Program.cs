using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuthenticationSystem.Models;
using UserAuthenticationSystem.Repositories;
using UserAuthenticationSystem.Services;
using UserAuthenticationSystem.Types;

namespace UserAuthenticationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<UserAuthenticationSystemDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDb")));

            builder.Services.AddScoped<IHashAlgoRepo, HashAlgoRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IJWTTokenServices, JWTTokenServices>();

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(CustomAuthenticationTypes.BasicAuthentication, null);
            builder.Services.AddAuthentication(k =>
            {
                k.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                k.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(p =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["JWTToken:this is my custom Secret key for authentication"]);
                p.SaveToken = true;
                p.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWKToekn:this is my custom Secret key for authentication"],
                    ValidAudience = builder.Configuration["JWKToekn:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers()
                .RequireAuthorization();

            app.Run();
        }
    }
}