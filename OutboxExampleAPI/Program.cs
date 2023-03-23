using Microsoft.EntityFrameworkCore;
using OutboxExample.Contracts.Persistence;
using OutboxExampleAPI;
using Serilog.Events;
using Serilog;
using System.Reflection;
using OutboxExample.Contracts.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    opt.UseNpgsql(connectionString, cfg =>
    {
        cfg.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        cfg.MigrationsHistoryTable($"__{nameof(AppDbContext)}");

        cfg.EnableRetryOnFailure(2);
        cfg.MinBatchSize(1);
    });
});
builder.Services.AddHostedService<DatabaseMigratorHostedService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddMassTransit(conf =>
{
    conf.AddEntityFrameworkOutbox<AppDbContext>(opt =>
    {
        opt.QueryDelay=TimeSpan.FromSeconds(1);
        opt.UsePostgres();
        opt.UseBusOutbox();
    });
    conf.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.AutoStart = true;
    });
});
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

app.Run();
