
using Burgers.Core.Interfaces;
using Burgers.Data;
using Burgers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

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
                });


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
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


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
