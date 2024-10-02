using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CSGOSkinApp.Data;
using Microsoft.Extensions.FileProviders;

var webApplicationOptions = new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "public")
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use static files
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Serve Svelte app for any non-API routes
app.MapFallbackToFile("index.html");

app.Run();