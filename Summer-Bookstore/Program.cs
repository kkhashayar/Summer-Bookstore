using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore.Application.Services;
using Summer_Bookstore.Application.Settings;
using Summer_Bookstore.Mappers;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registering DbContext
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registering Repositories
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Registering user service 
builder.Services.AddScoped<IUserService, UserService>();

// Register automapper.
// Fix for CS1503: Argument 2: cannot convert from 'System.Reflection.Assembly' Code pilot help :D
// to 'System.Action<AutoMapper.IMapperConfigurationExpression>'
builder.Services.AddAutoMapper(config => { config.AddMaps(typeof(BookMappers).Assembly); });
builder.Services.AddAutoMapper(config => { config.AddMaps(typeof(AuthorMappers).Assembly); });
builder.Services.AddAutoMapper(config => { config.AddMaps(typeof(UserRegisterMappers).Assembly); });


// Registering JwSettings 
// This tells .NET to look inside appsettings.json for a section called "JwtSettings" and bind it to JwSettings class.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwSettings"));

// Controllers asnd swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
