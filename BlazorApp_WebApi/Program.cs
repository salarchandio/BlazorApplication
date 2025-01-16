using BlazorApp_WebApi.Hubs;
using Common;
using DataAccessLayer;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<DAL>();
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

app.Run();
