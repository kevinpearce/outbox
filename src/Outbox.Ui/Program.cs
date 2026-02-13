using Outbox.Common;
using Outbox.Ui;
using Outbox.Ui.Hubs;
using Outbox.Ui.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLoggingService();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
});

builder.Services.AddScoped<ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCors();
app.UseAntiforgery();

app.MapHub<OutboxHub>("/outboxhub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
