using System.ComponentModel.DataAnnotations;
using System;

namespace FitnessApp.DTOs
{
    public class UpdateMember
    {
        public string PersonEmail { get; set; }
        public string GymBundleId { get; set; }
        public string PersonId { get; set; }
        public string MemberId { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        //public int ExpectedWeight { get; set; }
        //public string BMIStatus { get; set; }
        //public bool IsMemberOverWeight { get; set; } = false;

        public DateTime MembershipFrom { get; set; }
        public DateTime MembershipTo { get; set; }
    }

}
