using EasyTalk.Shared.Services;
using EasyTalk.Web.Components;
using EasyTalk.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HttpClient>(); // atau gunakan HttpClientFactory
builder.Services.AddScoped<APIServices>();
builder.Services.AddHttpClient<APIServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7177/"); // sesuaikan port backend
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the EasyTalk.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(EasyTalk.Shared._Imports).Assembly,
        typeof(EasyTalk.Web.Client._Imports).Assembly);

app.Run();
