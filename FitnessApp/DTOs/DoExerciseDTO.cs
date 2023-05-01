using FitnessApp.Models;

namespace FitnessApp.DTOs
{
    public class DoExerciseDTO
    {
        public string DayOfWeek { get; set; }
        public string ExerciseTitle { get; set; }
        public string ExerciseDuration { get; set; }
        public int  ExerciseCount { get; set; }
        public string  ExerciseTime { get; set; }

    }

}
