using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Middlewares;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog Console to get information on console window
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    // To get log in text file opion above line sufficient ot handle the log
    .WriteTo.File("Logs/Booking_logs.txt",rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Information()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();

// to upload image on server
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Open the Authorization Bearer Token in Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking.com API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Add DbContext class to connect with database and reate connectin string
builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BookingConnectionString")));
// cloudinary service
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
// Email notification
builder.Services.Configure<Booking.Com_Clone_API.Models.Domain.EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
// Injected Repository
// Use nuge package AutoMapper Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICloudinaryImageRepository, CloudinaryImageRepository>();
builder.Services.AddScoped<IAddHotelRepository, AddHotelRepository>();

// Injectd AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//============================= Start Jwt Authentication =================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    });
//============================= End Jwt Authentication =================

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handler
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

// Start Enabled Cors to acees any endpoint 
// WithExposedHeaders("*") to all headers
app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
// Start Enabled Cors to acees any endpoint

// Authentication
app.UseAuthentication();

app.UseAuthorization();
// to access the local file path using URL
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllerRoute(    
        name: "verify-email",
        pattern: "verify-email/{email}/{token}",
        defaults: new { controller = "User", action = "VerifyEmailAsync" });
app.MapControllerRoute(
        name: "reset-password",
        pattern: "reset-password/{email}/{token}",
        defaults: new { controller = "User", action = "ResetPassword" });

app.MapControllers();

app.Run();
