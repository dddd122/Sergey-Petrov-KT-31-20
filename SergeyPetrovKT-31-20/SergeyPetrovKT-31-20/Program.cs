using SergeyPetrovKT_31_20.Database;
using SergeyPetrovKT_31_20.interfaces.StudentInterfaces;
//using SergeyPetrovKT_31_20.Middlewares;
using SergeyPetrovKT_31_20.ServiceExtensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using static SergeyPetrovKT_31_20.ServiceExtensions.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
// Add services to the container.

try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<SerDbContext>(
    builder.Configuration.GetSection(nameof(SerDbContext)));
    IServiceCollection serviceCollection = builder.Services.AddDbContext<DbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
      //builder.Services.AddScoped<IDataAccessProvider, DataAccessProvider>();
    builder.Services.AddDbContext<SerDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefultConnection")));
   builder.Services.AddScoped<IStudentService, StudentFilterService>();
    builder.Services.AddService();
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}

catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
}
finally
{
    LogManager.Shutdown();
}
