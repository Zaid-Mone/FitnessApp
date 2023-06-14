using FitnessApp.Models;
using System.Collections.Generic;
using System;

namespace FitnessApp.DTOs
{
    public class DisplayMemberDTO
    {


        public string MemberName { get; set; }


        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string GymBundleName { get; set; }

        public ICollection<TrainersMember> TrainersMembers { get; }


        public int ExpectedWeight { get; set; }
        public string BMIStatus { get; set; }
        public bool IsMemberOverWeight { get; set; } = false;

        public DateTime MembershipFrom { get; set; } // DateTime.Now
        public DateTime MembershipTo { get; set; } // MembershipFrom.AddDays(=> get this form the gym bundle)




        public string TrainerName { get; set; }
    }

}
