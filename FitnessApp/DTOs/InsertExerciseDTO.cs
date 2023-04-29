using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class InsertExerciseDTO
    {
        [Required]
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Exercise Count")]
        public int ExerciseCount { get; set; } = 0;
        [Display(Name = "Exercise Duration")]
        public string ExerciseDuration { get; set; }
        [Required, DataType(DataType.Date)]
        [Display(Name = "Date Of Exercise")]
        public DateTime DateOfExercise { get; set; }

        [Required, DataType(DataType.Time)]
        [Display(Name = "Exercise Start From")]
        public TimeSpan ExerciseFrom { get; set; }
        [Required, DataType(DataType.Time)]
        [Display(Name = "Exercise End At")]
        public TimeSpan ExerciseTO { get; set; }
    }
}
