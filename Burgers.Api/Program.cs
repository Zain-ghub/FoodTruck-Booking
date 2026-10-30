
using Burgers.Core.Interfaces;
using Burgers.Services;
using Microsoft.EntityFrameworkCore;
using Burgers.Data;
namespace Burgers.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
           
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            
            builder.Services.AddDbContext<BurgersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

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
            


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
