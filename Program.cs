using System.Text;
using LibraryManagement.Core.Abstractions;
using LibraryManagement.Persistence;
using LibraryManagement.Persistence.Repositories;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Token"]!)
            )
        };
    });

builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MembersRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();

var app = builder.Build();

/* using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    context.Database.Migrate();
    await ApplicationDbSeeder.SeedAsync(context);
} */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();