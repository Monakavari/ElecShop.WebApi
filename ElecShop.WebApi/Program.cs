using AngularEshop.Core.Services.Implementations;
using ElecShop.Core.Services.Contracts;
using ElecShop.Core.Services.Implementation;
using ElecShop.WebApi.Core.Security;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Services.Implementation;
using ElecShop.WebApi.Core.Utilities.Convertors;
using ElecShop.WebApi.Core.Utilities.Extensions.Connection;
using ElecShop.WebApi.DataLayer.Context;
using ElecShop.WebApi.DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ElecShopDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("ElecShopConnectionString")

));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService >();
builder.Services.AddScoped<IMailSender, SendEmail>();
builder.Services.AddScoped<IViewRenderService, RenderViewToString>();

#region Add DbContext

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

#endregion

#region Application Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

#endregion

#region Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:44381",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"))
        };
    });

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .Build();
    });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("EnableCors");
app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
