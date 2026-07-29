using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Infrastructure.Repositories;
using FacilityInspectionTracker.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Technical Debt: Hardcoded connection string (would be used if we had a real DB)
var connectionString = "Server=localhost;Database=FacilityInspectionDb;User Id=sa;******;";
builder.Services.AddSingleton<string>(connectionString);

// Add services to the container.
builder.Services.AddControllers();

// Register repositories (singleton to preserve in-memory state)
builder.Services.AddSingleton<IFacilityRepository, FacilityRepository>();
builder.Services.AddSingleton<IInspectionRepository, InspectionRepository>();

// Register services
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Facility Inspection Tracker API",
        Version = "v1",
        Description = "API for managing TDCJ facility inspections and scheduling.",
        Contact = new OpenApiContact
        {
            Name = "TDCJ Facilities Division",
            Email = "facilities@tdcj.texas.gov"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Technical Debt: Accessibility issue - Swagger UI custom page is missing
    // lang attribute on the <html> element, causing screen reader issues
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Facility Inspection Tracker API v1");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "Facility Inspection Tracker";
        c.InjectStylesheet("/swagger-ui/custom.css");
        // Missing: HeadContent with <html lang="en"> - accessibility issue
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expose Program class for WebApplicationFactory in integration tests
public partial class Program { }
