using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class CurveModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CurveType { get; set; } = string.Empty;

        [Required]
        public string Equation { get; set; } = string.Empty;

        // Navigation property for associated points
        public List<PointModel> Points { get; set; } = new List<PointModel>();
    }
}
