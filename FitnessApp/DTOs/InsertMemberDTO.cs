using FitnessApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class InsertMemberDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Trainer")]
        public string TrainerId { get; set; }

        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Gym Bundle")]
        public string GymBundleId { get; set; }

    }
}
