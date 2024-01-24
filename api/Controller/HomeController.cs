using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    [HttpPost("curve")]
        public IActionResult CreateCurve([FromBody] CurveModel curve)
        {
            try
            {
                if (curve == null || curve.Points == null || curve.Points.Count == 0)
                {
                    return BadRequest("Invalid curve data");
                }

                // Save the curve and its associated points to the database
                _dbContext.Curves.Add(curve);
                _dbContext.SaveChanges();

                return Ok("Curve saved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

[HttpGet]
public IActionResult GetCurves()
{
    try
    {
        // Retrieve all curves with their associated points from the database
        var curves = _dbContext.Curves
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

        return Ok(shapedCurves);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
    }
}
    
        }
    }