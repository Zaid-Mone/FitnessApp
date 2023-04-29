using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class TrainersMember
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("MemberId")]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        [ForeignKey("TrainerId")]
        public string TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        //[Key]
        //public string NutritionId { get; set; }
        //public Nutrition Nutrition { get; set; }

    }

}
