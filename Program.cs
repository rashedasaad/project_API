using System.Text;
using DotNetEnv;
using FargoApi.Services.Whatsapp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using project_API.Repositories;
using Swashbuckle.AspNetCore.Filters;
using project_API.Services.Auth;
using project_API.Services.Payment;
using project_API.Services.Payment.Services;
using IPaymentManager = project_API.Services.Payment.Services.IPaymentManager;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "oauth2",
        new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }
    );
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add DbContext
builder.Services.AddDbContext<appdbcontext>();

builder.Services.AddHttpContextAccessor();
// Add HttpClientFactory
builder.Services.AddHttpClient(); // Required for IHttpClientFactory in PaymentManager

// Register repositories and services
builder.Services.AddTransient(typeof(IdataHelper<>), typeof(DataHelper<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();
builder.Services.AddSingleton<WhatsAppManger>();
builder.Services.AddSingleton<AuthManger>();
builder.Services.AddScoped<TokenRepository>();
builder.Services.AddSingleton<ClaimsReader>();
// Configure JWT authentication
builder.Services
    .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = Env.GetString("JWT_AUDIENCE"),
        ValidIssuer = Env.GetString("JWT_ISSUER"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET")))
    });

// Configure CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true);
    })
);

var app = builder.Build();

app.UseCors();

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();