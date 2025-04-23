using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EC2_2100212.Web.Configurations;
using EC2_2100212.Web.Interfaces;
using EC2_2100212.Web.Implementations;
using EC2_2100212.Repositories.Data;
using EC2_2100212.Models;
using EC2_2100212.Services.Implementations;
using EC2_2100212.Repositories.Interfaces;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BagsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BagDbConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<BagsDbContext>().AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(MapperInitializer));

builder.Services.AddTransient<IAuthManager, AuthManager>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IGenericService<CategoryViewModel>, CategoryService>();
builder.Services.AddTransient<IGenericService<MaterialViewModel>, MaterialService>();
builder.Services.AddTransient<IGenericService<BagCategoryViewModel>, BagCategoryService>();
builder.Services.AddTransient<IGenericService<BagMaterialViewModel>, BagMaterialService>();
builder.Services.AddTransient<IGenericService<BagViewModel>, BagService>();
builder.Services.AddTransient<IGenericService<OrderViewModel>, OrderService>();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
