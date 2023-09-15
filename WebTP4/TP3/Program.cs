using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TP3.Data;
using TP3.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TP3Context>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("TP3Context") ?? throw new InvalidOperationException("Connection string 'TP3Context' not found."));
options.UseLazyLoadingProxies();
});
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<TP3Context>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GalerieService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow all", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "http://localhost:4200",
        ValidIssuer = "https://localhost:7171",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Loo00ongue Phrase SiNoN Ça ne Marchera PaAaAAAaAas !"))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("Allow all");

app.UseAuthorization();

app.MapControllers();

app.Run();
