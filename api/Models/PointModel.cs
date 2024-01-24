using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class PointModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        // Foreign key to associate the point with a curve
        public int CurveId { get; set; }

        // Navigation property for the associated curve
        public CurveModel Curve { get; set; } 
    }
}