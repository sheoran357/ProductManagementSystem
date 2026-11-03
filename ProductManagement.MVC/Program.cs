using ProductManagement.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductApiService, ProductApiService>(client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"]
        ?? throw new InvalidOperationException("API base URL is missing.");

    client.BaseAddress = new Uri(apiUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
