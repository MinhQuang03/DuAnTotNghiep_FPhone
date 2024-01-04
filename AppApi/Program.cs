using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AppApi;
using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<FPhoneDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<FPhoneDbContext>(options => {
    options.UseSqlServer(@builder.Configuration.GetConnectionString("PRO219_WebsiteBanDienThoai"), opt =>opt.EnableRetryOnFailure());
});

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      builder =>
//                      {
//                          builder.WithOrigins("https://localhost:7171/");
//                      });
//});

ServiceRegistration.Configure(builder.Services);

var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        //Tự cấp token
        ValidateIssuer = false,
        ValidateAudience = false,

        //Ký vào token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),

        ClockSkew = TimeSpan.Zero
    };
    //opt.Authority = "https://login.microsoftonline.com/5ac07b9d-f4eb-4eca-9eb0-9dcf18c3dcc8";
    //opt.Audience = "adc439cf-7627-4a2c-8ef4-67bcc2975b55";

});




var app = builder.Build();
app.UseCors(options =>
{
	options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.Run();


