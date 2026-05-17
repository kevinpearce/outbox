using FluentValidation;
using Outbox.Api.Configuration;
using Outbox.Application;
using Outbox.Application.Interfaces;
using Outbox.Common;
using Outbox.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLoggingService();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.Configure<ValidationSettings>(builder.Configuration.GetSection("ValidationSettings"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOutboxService, OutboxService>();

builder.Services.AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.UseInfrastructure();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();