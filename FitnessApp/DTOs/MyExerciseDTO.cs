using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class MyExerciseDTO
    {

        public string Title { get; set; }
        [Display(Name = "Exercise Count")]
        public int ExerciseCount { get; set; } = 0;
        [Display(Name = "Exercise Duration")]
        public string ExerciseDuration { get; set; } = "";
        [Display(Name = "Date Of Exercise")]
        [DataType(DataType.Date)]
        public DateTime DateOfExercise { get; set; }
        [Display(Name = "Exercise From")]
        public TimeSpan ExerciseFrom { get; set; }
        [Display(Name = "Exercise TO")]
        public TimeSpan ExerciseTO { get; set; }
        [Display(Name = "Exercise Time")]
        public string ExerciseTimeFormat { get; set; }

    }

}
