
using Burgers.Api.Hubs;
using Burgers.Core.Interfaces;
using Burgers.Data;
using Burgers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Burgers.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
           
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{{new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, new string[] { }}});

            });
            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddSignalR();


            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

            builder.Services.AddCors(options => { options.AddPolicy("Angular", policy => { policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }); });
            

            builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<BurgersContext>();
            
            builder.Services.AddDbContext<BurgersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var key = builder.Configuration["Jwt:Key"]
               ?? throw new InvalidOperationException("Jwt:Key is missing");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!String.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) { context.Token = accessToken; }
                            return Task.CompletedTask;
                        }
                    };
                });


            var app = builder.Build();

           

            

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BurgersContext>();
                var runMigrations = app.Configuration.GetValue<bool>("RunMigrationsOnStartup");
                if (runMigrations) { context.Database.Migrate(); }
               var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var email = app.Configuration["Admin:Email"];
                var password = app.Configuration["Admin:Password"];
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    var existing = await userManager.FindByEmailAsync(email);
                    if (existing == null)
                    {
                        var user = new IdentityUser { UserName = email, Email = email };
                        var result = await userManager.CreateAsync(user, password);
                        if (result.Succeeded == false)
                        {
                            foreach (var error in result.Errors)
                            {
                                app.Logger.LogError(error.Description);
                            }
                        } else
                        {
                            app.Logger.LogInformation("Admin user created");
                        }
                    }
                }
                else
                {
                    app.Logger.LogWarning("Admin credentials not configured");
                }

            }

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseExceptionHandler();
            app.UseCors("Angular");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<BookingsHub>("/hubs/bookings");

            app.MapControllers();

            app.Run();
        }
    }
}
