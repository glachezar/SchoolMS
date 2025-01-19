namespace WebApi
{
    using Infrastructure;
    using Infrastructure.Persistence;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Database initialization
            await app.Services.AddDbInitializerAsync();

            // Configure the HTTP request pipeline.


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
