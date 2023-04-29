using FitnessApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Member
    {
        [Key]
        public string Id { get; set; }


        [ForeignKey("PersonId")]
        public string PersonId { get; set; }
        public Person Person { get; set; }

        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [ForeignKey("GymBundleId")]
        [Display(Name = "Gym Bundle")]
        public string GymBundleId { get; set; }
        public GymBundle GymBundle { get; set; }
        public ICollection<TrainersMember> TrainersMembers { get; }

         
        public int ExpectedWeight { get; set; }
        public string BMIStatus { get; set; }
        public bool IsMemberOverWeight { get; set; } = false;


        //public string Country { get; set; } = "Jordan";   
        //public State State { get; set; }
        //public string Address { get; set; } 
    }

}
