using Outbox.Api.Minimal.Middleware;
using Outbox.Api.Minimal.Outbox;
using Outbox.Api.Minimal.User;
using Outbox.Common;
using Outbox.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddLoggingService();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructure();
builder.Services.AddUserServices(builder.Configuration);
builder.Services.AddOutboxServices();

var app = builder.Build();

await app.Services.UseInfrastructure();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapOutboxEndpoints();

app.MapOpenApi();

app.Run();
