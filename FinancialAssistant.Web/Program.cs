var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorPages();

builder.Services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapRazorPages();
});

app.Run();
