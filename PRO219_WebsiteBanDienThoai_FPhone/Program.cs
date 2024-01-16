using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IVwPhoneService, VwPhoneService>();
builder.Services.AddSingleton<FPhoneDbContext>();
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
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IBlogService, BlogService>();
builder.Services.AddTransient<IVwTop5PhoneServices, VwTop5PhoneService>();
builder.Services.AddTransient<IBillRepository, BillRepository>();
builder.Services.AddTransient<IvOverViewServices, OverViewServices>();
builder.Services.AddTransient<IBillGanDayServices, BillGanDayServices>();
builder.Services.AddTransient<IPhoneStatiticsServices, PhoneStatiticsServices>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<FPhoneDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped(sp => new HttpClient()
{
    //Uri chạy iis
    BaseAddress = new Uri("https://localhost:44373/")

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
builder.Services.AddApplicationInsightsTelemetry();


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
