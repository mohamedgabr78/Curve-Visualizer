using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
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



app.MapGet("/api/home", async (AppDbContext dbContext) =>
{
    try
    {
        // Retrieve all curves with their associated points from the database
        var curves = dbContext.Curves
            .Include(c => c.Points)
            .ToList();

        // Shape the response
        var shapedCurves = curves.Select(c => new
        {
            c.Id,
            c.CurveType,
            c.Equation,
            Points = c.Points.Select(p => new { X = p.X, Y = p.Y }).ToList()
        });

        return Results.Ok(shapedCurves);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Internal Server Error: {ex.Message}");
    }
    {
    }
});


app.MapPost("/api/home/curve", async (AppDbContext dbContext, CurveModel curve) =>
{
        if (curve == null || curve.Points == null || curve.Points.Count == 0)
        {
            return Results.BadRequest("Invalid curve data");
        }


        // Save the curve and its associated points to the database
        dbContext.Curves.Add(curve);
        await dbContext.SaveChangesAsync();

        return Results.Ok("Curve saved successfully");
});


app.UseCors();
app.Run();

