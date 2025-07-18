using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Summer_Bokkstore_Infrastructure.Interfaces;
using Summer_Bookstore.Application.Services;
using Summer_Bookstore.Application.Settings;
using Summer_Bookstore.Mappers;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.Repositories;
using System.Text;





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
builder.Services.AddAutoMapper(config => { config.AddMaps(typeof(UserRegAndReadMappers).Assembly); });

// Above code I am not sure if only one mapper assembly would be enough! 


// Registering JwSettings 
// This tells .NET to look inside appsettings.json for a section called "JwtSettings" and bind it to JwSettings class.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registering TokenService 
builder.Services.AddScoped<ITokenService, TokenService>();


// Configure authentication // most complicated part so far.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

// Role based plicies: 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});


// Controllers asnd swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Configure Swagger to use JWT Authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Summer bookstore", Version = "v1" });

    // JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token without quotes. Example: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
