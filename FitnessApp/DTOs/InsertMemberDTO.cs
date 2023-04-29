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


        public string TrainerId { get; set; }

        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string GymBundleId { get; set; }

    }
}
