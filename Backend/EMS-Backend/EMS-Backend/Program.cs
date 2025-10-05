using EMS_Backend.Data;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using EMS_Backend.Repository;
using EMS_Backend.Seed;
using EMS_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();




//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// dependency inject for department
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// dependency inject for employee

builder.Services.AddScoped<IEmployeeRespository, EmployeeRepository>();
//builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();



//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//        policy =>
//        {
//            policy.AllowAnyOrigin();
//               policy.AllowAnyMethod();
//            policy.AllowAnyHeader();
//        });
//});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    //this code change according to needs 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

// JWT Authentication
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);/

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["JWT:Issuer"],
//        ValidAudience = builder.Configuration["JWT:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//    };
//});



builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();   // add this
    app.MapOpenApi();
    app.MapScalarApiReference();
}


// ?? Use CORS before MapControllers
app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)

);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
