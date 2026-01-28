using CsvImportDemo.Data;
using CsvImportDemo.Repositories;
using CsvImportDemo.Services;
using CsvImportDemo.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure SQLite DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=products.db"));

// Register dependencies
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductValidator, ProductValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.OperationFilter<CsvImportDemo.AddFileUploadOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CSV Import API 🚀",
        Version = "v1",
        Description = "📂 CSV file upload importer\n🗂 SQLite-backed storage\n⚙️ Built with ASP.NET Core Web API\n🧪 Explore and try endpoints via Swagger UI"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "CSV Import API | Swagger";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CSV Import API v1");
        options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

// app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();

app.Run();
