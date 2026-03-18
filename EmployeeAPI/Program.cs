using EmployeeAPI.Data;
using EmployeeAPI.Domain.Data;
using EmployeeAPI.Repository;
using EmployeeAPI.Repository.Interfaces;
using EmployeeAPI.Services;
using EmployeeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Connection string ophalen uit appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// DbContexts registreren
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<SwaggerDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ASP.NET Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Swagger configuratie
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API Employee",
        Version = "version 1",
        Description = "An API to perform Employee operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "CDW",
            Email = "christophe.dewaele@vives.be",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "Employee API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

// AutoMapper registreren
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency Injection (DAO + Services)
builder.Services.AddScoped<IEmployeeDAO, EmployeeDAO>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//Gebruik JWT Bearer authentication als standaard authenticatiemethode.
//ASP.NET verwacht een header zoals: Authorization: Bearer<token>
builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //Gebruik JWT Bearer authentication als standaard authenticatiemethode.
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Configureer JWT Bearer authentication
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    //Configureer de parameters voor het valideren van het token
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:JwtIssuer"], // uitgever van het token
        ValidAudience = builder.Configuration["JwtConfig:JwtIssuer"],
        //de sleutel waarmee de token signature wordt gecontroleerd
        IssuerSigningKey = new
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:JwtKey"])),
        ClockSkew = TimeSpan.Zero // remove delay of token when expire
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminManager", policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("functie", "Manager");
    });
});

var app = builder.Build();

// HTTP request pipeline configureren
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Swagger instellingen ophalen uit appsettings.json
var swaggerOptions = new EmployeeAPI.Options.OptionsSwagger();
builder.Configuration.GetSection(nameof(EmployeeAPI.Options.OptionsSwagger)).Bind(swaggerOptions);

// Swagger middleware
app.UseSwagger(option =>
{
    option.RouteTemplate = swaggerOptions.JsonRoute;
});

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});

app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();

// Controllers activeren (nodig voor API endpoints)
app.MapControllers();

// Razor pages en MVC routes
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();