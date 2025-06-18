using DataAcess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Models.DTOs.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using DataAcess.Repos;
using DataAcess.Repos.IRepos;
using Models.Domain;
using Microsoft.Extensions.FileProviders;
using IdentityManagerAPI.Middlewares;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManager.Services.ControllerService;
using Models.DTOs.EmailSender;
using Models.DTOs;
using IdentityManagerAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//AddEmailSender
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//ChatBot
var openAiKey = builder.Configuration["OpenAIKey"];
builder.Services.AddHttpClient();
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenApi"));


//signalR
builder.Services.AddSignalR();
// ده لو عايزة تشتغلي بالـ Identity UserId كـ ConnectionId
//builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


//cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            //.WithOrigins("http://localhost:3000") // غيّري حسب الفرونت
            //.AllowAnyHeader()
            //.AllowAnyMethod()
            //.AllowCredentials();
            .AllowAnyOrigin() // <-- مؤقتًا نسمح لكل الـ Origins
            .AllowAnyHeader()
            .AllowAnyMethod();
            //.AllowCredentials();
    });
});





// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();


// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IMessage, MessageRepo>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointment, AppointmentRepository>();

// Add OpenAPI with Bearer Authentication Support
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});



// Configure JWT Authentication insted of cookies
var key = Encoding.ASCII.GetBytes(builder.Configuration["ApiSettings:Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromDays(7)
    };
});


// Register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Use the global exception handler
app.UseExceptionHandler();


app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
//    RequestPath = "/Images"
//});

app.MapControllers();

app.Run();
