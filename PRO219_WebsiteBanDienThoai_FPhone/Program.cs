using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Repositories;
using AppData.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IVwPhoneService, VwPhoneService>();
builder.Services.AddTransient<FPhoneDbContext>();
builder.Services.AddTransient<IVwPhoneDetailService,VwPhoneDetailService>();
builder.Services.AddTransient<IListImageService,ListImageService>();
builder.Services.AddTransient<IBlogRepository,BlogRepository>();
builder.Services.AddTransient<IPhoneRepository,PhoneRepository>();
builder.Services.AddTransient<ICartDetailService, CartDetailService>();
builder.Services.AddTransient<IProductionCompanyRepository, ProductionCompanyRepository>();
builder.Services.AddTransient<IRamRepository, RamRepository>();
builder.Services.AddTransient<IRomRepository, RomRepository>();
builder.Services.AddTransient<IRanksRepositories, RankRepositories>();
builder.Services.AddTransient<IChipCPURepository, ChipCPURepository>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();


builder.Services.AddScoped(sp => new HttpClient()
{
    BaseAddress = new Uri("https://localhost:7129/")
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60); // dinh nghia session ton tai
});
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = new PathString("/home");
    });


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
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "areas",
        "{area:exists}/{controller=LogIn}/{action=Login}/{id?}");
    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
