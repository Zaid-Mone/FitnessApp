using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class GymBundle
    {
        [Key]
        public string Id { get; set; }
        public string BundleTitle { get; set; }
        public decimal Price { get; set; }
    }

}
