using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers and Razor views
builder.Services.AddControllersWithViews();

// Session configuration for login state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// In-memory data provider for FAQs and opening hours fallback
builder.Services.AddSingleton<IMuseumData, InMemoryMuseumData>();

// Azure SQL database connection via Entity Framework Core
builder.Services.AddDbContext<MuseumDbContext>(options =>
    options.UseSqlServer(
        "Server=tcp:kslat-museum-server.database.windows.net,1433;" +
        "Initial Catalog=MuseumDb;" +
        "Persist Security Info=False;" +
        "User Id=museumadmin;" +
        "Password=pass123//;" +
        "MultipleActiveResultSets=False;" +
        "Encrypt=True;" +
        "TrustServerCertificate=False;" +
        "Connection Timeout=30;",
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    ));

var app = builder.Build();

// Seed the database with initial event and opening hour data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MuseumDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

// HTTP pipeline configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();