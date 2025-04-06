
using Microsoft.EntityFrameworkCore;
using MyWardrobeStatistics.Persistence;

namespace MyWardrobeStatistics
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<MyWardrobeStatisticsDbContext>(options =>
                options.UseSqlite(@"Data Source=reading_stats.db"));

            builder.Services.AddSingleton<Receive>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            var receiveService = app.Services.GetRequiredService<Receive>();
            await receiveService.ListenForWardrobeItemUsedEvent();

            app.Run();
        }
    }
}
