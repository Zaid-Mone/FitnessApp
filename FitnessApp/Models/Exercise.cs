using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Exercise
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Exercise Count")]
        public int ExerciseCount { get; set; } = 0;

        [Display(Name = "Exercise Duration")]
        public string ExerciseDuration { get; set; } = "";
        [ForeignKey("MemberId")]
        [Required]
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date Of Exercise")]
        public DateTime DateOfExercise { get; set; }

        [Required, DataType(DataType.Time)]
        [Display(Name = "Exercise Start From")]
        public TimeSpan ExerciseFrom { get; set; }
        [Required, DataType(DataType.Time)]
        [Display(Name = "Exercise End At")]
        public TimeSpan ExerciseTO { get; set; }
        [Display(Name = "Exercise Time")]
        public string ExerciseTimeFormat { get; set; }
    }


}
