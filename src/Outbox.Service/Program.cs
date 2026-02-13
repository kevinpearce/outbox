using Outbox.Common;
using Outbox.Service;
using Outbox.Application.Interfaces;
using Outbox.Application;
using Outbox.Infrastructure;
using Outbox.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Outbox.Service.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLoggingService();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IOutboxService, OutboxService>();

builder.Services.Configure<UiOptions>(builder.Configuration.GetSection("Ui"));
builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection("Worker"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();