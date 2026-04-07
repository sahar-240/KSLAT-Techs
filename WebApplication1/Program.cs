using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ ADD SESSION SERVICES
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // For HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Front-end only data provider (for opening hours, FAQs etc)
builder.Services.AddSingleton<IMuseumData, InMemoryMuseumData>();


builder.Services.AddScoped<IPaymentService, MockPaymentService>();

// SQL Database connection for Events and Bookings
builder.Services.AddDbContext<MuseumDbContext>(options =>
    options.UseSqlServer(
        "Server=tcp:kslat-museum-server.database.windows.net,1433;" +
        "Initial Catalog=MuseumDb;" +
        "User ID=museumadmin;" +
        "Password=pass123//;" +
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

// Seed the database with events if empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MuseumDbContext>();
    db.Database.EnsureCreated();

    DbSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ADD SESSION MIDDLEWARE (MUST BE BEFORE UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();