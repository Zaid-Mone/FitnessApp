using FitnessApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class RegistrationNotification
    {

        /// <summary>
        /// This class is that the member can send his data and the admin can recive it and member without 
        /// member be in the gym (that mean the Admin can register a new member online)
        /// </summary>

        [Key]
        public string Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Required]
        [Display(Name = "Weight")]
        public float Weight { get; set; }
        [Required]
        [Display(Name = "Height")]
        public float Height { get; set; }

        [ForeignKey("GymBundleId")]
        [Display(Name = "Gym Bundle")]
        public string GymBundleId { get; set; }
        public GymBundle GymBundle { get; set; }

        [ForeignKey("TrainerIdId")]
        [Display(Name = "Trainer")]
        public string TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }     

        //public DateTime DateOfRegisteration { get; set; }
        //public string  Status { get; set; } // approved // deined
        //public bool isApproved { get; set; } // this mean if the user has been approved mean he has a account in users table so don't show him again in the list 
        //public bool IsDeleted { get; set; } = false;
    }

}
