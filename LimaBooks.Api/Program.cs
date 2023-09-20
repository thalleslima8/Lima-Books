using Serilog.Events;
using Serilog;
using LimaBooks.Repository;
using Microsoft.EntityFrameworkCore;
using LimaBooks.Repository.Interfaces;
using LimaBooks.Repository.Repositories;
using LimaBooks.Service.BookService;
using LimaBooks.Service.MapperService;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
ConfigDataBase(builder);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Limpar os provedores de log existentes
    loggingBuilder.AddSerilog(dispose: true); // Adicionar o Serilog como provedor de log
});

// Add services to the container.
builder.Services.AddScoped<IBookService, BookService>();


//Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperService));

var app = builder.Build();

// Ensure the database is created.
EnsureDBCreated(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigDataBase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<MySqlContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)))
        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information));
}

static void EnsureDBCreated(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<MySqlContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}