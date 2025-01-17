using System.Security.Cryptography;
using BlazorApp_WebApi.Hubs;
using BlazorApp_WebApi.JWT;
using Common;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

string? _KeyDirectory = builder.Configuration.GetSection("KeySettings:KeyDirectory").Value;
string? _publicKeyFileName = builder.Configuration.GetSection("KeySettings:PublicKeyFileName").Value;
string? _privateKeyFileName = builder.Configuration.GetSection("KeySettings:PrivateKeyFileName").Value;

string _publickey = Path.Combine(_KeyDirectory, _publicKeyFileName);
string _privatekey = Path.Combine(_KeyDirectory, _privateKeyFileName);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
           
               byte[]? publicKey = Convert.FromBase64String(File.ReadAllText(_publickey)); 
               var rsa = RSA.Create();
               rsa.ImportRSAPublicKey(publicKey, out _);

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidIssuer = "Vender", 
                   ValidAudience = "Client",
                   ValidateLifetime = true,
                   IssuerSigningKey = new RsaSecurityKey(rsa),
                   ValidateIssuerSigningKey = true
               };
           });

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddSingleton<DAL>();
builder.Services.AddSingleton<GenerateToken>();

builder.Services.AddScoped<ChatHub>();
builder.Services.AddScoped<BaseRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<StampFieldsRepository>();
builder.Services.AddScoped<UserPasswordRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserRoleRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<UserPasswordService>();
builder.Services.AddScoped<StampFieldsService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();


// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Example of a more restricted policy
    options.AddPolicy("SpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:7178")  // https://anotherdomain.com Add allowed origins
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Enable the CORS middleware
app.UseCors("AllowAll"); // Use the defined policy

// Or specify the policy name when needed
//app.UseCors("SpecificOrigins");

app.MapHub<ChatHub>("/chathub");

app.UseAuthorization();

app.MapControllers();

// This is a bad practice i know but kindof in a hurry 
if (!File.Exists(_privatekey) || !File.Exists(_publickey))
{
    Encryption.GenerateRsaKeys(_privatekey, _publickey);
}

app.Run();
