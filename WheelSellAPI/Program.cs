using Microsoft.EntityFrameworkCore;
using WheelSell.DAL;
using WheelSell.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Регистрация сервисов для Dependency Injection

// 1. Регистрация контекста базы данных
// Получаем строку подключения из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Регистрация сервисов BLL
// Используем AddScoped, так как AppDbContext работает в рамках одного запроса
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserService, UserService>();

// 3. Добавление контроллеров
builder.Services.AddControllers();

// Добавляем Swagger для тестирования API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Настройка конвейера обработки запросов

// В режиме разработки используем Swagger для удобного тестирования
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Перенаправление HTTP-запросов на HTTPS
app.UseHttpsRedirection();

// Включаем авторизацию и аутентификацию (если нужно)
app.UseAuthorization();

// Маппинг контроллеров
app.MapControllers();

app.Run();