using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WheelSellTA.DAL;
using WheelSellTA.BLL.Services;
using WheelSellTA.DAL.Entities;
using AutoMapper;
using WheelSellTA.BLL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WheelSellTA.API;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// ... (Остальной код)

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SalerOrAdmin", policy => policy.RequireRole("Saler", "Admin"));
});

builder.Services.AddAutoMapper(typeof(CarProfile).Assembly);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<ModelService>();
builder.Services.AddScoped<FuelTypeService>();
builder.Services.AddScoped<TransmissionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// >>>>> ВРЕМЕННЫЙ КОД ДЛЯ СОЗДАНИЯ БАЗЫ ДАННЫХ <<<<<
// Этот код создаст базу данных с нуля на основе исправленной модели.
// Его нужно удалить после первого успешного запуска.
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        
        // EnsureCreatedAsync() создаст базу данных WheelSellDMLocal с нуля,
        // включая все таблицы, по вашей исправленной модели.
        await context.Database.EnsureCreatedAsync();

        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        await SeedData.Initialize(userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Выводит сообщение, если SQL Server недоступен или есть другие ошибки.
        Console.WriteLine($"An error occurred during DB creation: {ex.Message}");
    }
}
// >>>>> КОНЕЦ ВРЕМЕННОГО КОДА <<<<<

app.Run();