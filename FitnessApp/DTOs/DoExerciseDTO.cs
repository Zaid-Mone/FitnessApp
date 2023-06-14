using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
