using Microsoft.EntityFrameworkCore;
using WheelSell.DAL;
using WheelSell.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ����������� �������� ��� Dependency Injection

// 1. ����������� ��������� ���� ������
// �������� ������ ����������� �� appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. ����������� �������� BLL
// ���������� AddScoped, ��� ��� AppDbContext �������� � ������ ������ �������
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserService, UserService>();

// 3. ���������� ������������
builder.Services.AddControllers();

// ��������� Swagger ��� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// ��������� ��������� ��������� ��������

// � ������ ���������� ���������� Swagger ��� �������� ������������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������������� HTTP-�������� �� HTTPS
app.UseHttpsRedirection();

// �������� ����������� � �������������� (���� �����)
app.UseAuthorization();

// ������� ������������
app.MapControllers();

app.Run();