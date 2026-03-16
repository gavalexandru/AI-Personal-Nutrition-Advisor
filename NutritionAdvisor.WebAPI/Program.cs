using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NutritionAdvisor.Application.Auth.Commands.Register;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;
using NutritionAdvisor.Infrastructure;
using NutritionAdvisor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

//  Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClientPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7067", "http://localhost:5132") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

// Config JWT to be able to read from Cookie
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
        };
        
        // Extract token from cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwtToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
    context.Database.EnsureCreated();
    
    if (!context.Allergies.Any())
    {
        context.Allergies.AddRange(
            new Allergy("Peanuts"),               
            new Allergy("Tree Nuts"),             
            new Allergy("Milk (Dairy/Lactose)"),  
            new Allergy("Eggs"),                  
            new Allergy("Wheat (Gluten)"),        
            new Allergy("Soy"),                   
            new Allergy("Fish"),                  
            new Allergy("Shellfish"),             
            new Allergy("Sesame"),                
            new Allergy("Mustard"),               
            new Allergy("Celery"),                
            new Allergy("Sulfites"),              
            new Allergy("Lupin")                 
        );
    }
    
    if (!context.DietPreferences.Any())
    {
        context.DietPreferences.AddRange(
            new DietPreference(DietPreferenceType.Balanced),
            new DietPreference(DietPreferenceType.Vegetarian),
            new DietPreference(DietPreferenceType.Vegan),
            new DietPreference(DietPreferenceType.Pescatarian),
            new DietPreference(DietPreferenceType.Keto),
            new DietPreference(DietPreferenceType.Paleo),
            new DietPreference(DietPreferenceType.Carnivore),
            new DietPreference(DietPreferenceType.Mediterranean)
        );
    }

    context.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nutrition Advisor API");
    c.RoutePrefix = string.Empty; 
});

app.UseHttpsRedirection();
app.UseCors("BlazorClientPolicy"); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();