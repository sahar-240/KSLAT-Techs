using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Front-end only data provider (for opening hours, FAQs etc)
builder.Services.AddSingleton<IMuseumData, InMemoryMuseumData>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();