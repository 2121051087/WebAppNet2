using AppNet2.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAppNet2.Infrastructures.Repositories;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.Entities.Catalog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<AppNet2.Models.DbNet2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppNet2.Models.DbNet2Context>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache(); // Dịch vụ bộ nhớ cache cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của Session
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập qua HTTP
    options.Cookie.IsEssential = true; // Bắt buộc lưu trữ cookie
});
// 


builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IUnitOfWork, DbNet2UnitOfWork>();
builder.Services.AddScoped<IProductRepository , ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DbNet2Context>();
        Sizes.SeedDefaultSizes(context);
        Colors.SeedDefaultColors(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas_admin",
      pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "areas_customer",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
