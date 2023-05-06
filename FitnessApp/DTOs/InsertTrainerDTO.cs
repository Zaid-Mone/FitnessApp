using FitnessApp.Enums;
using FitnessApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class InsertTrainerDTO
    {
        public string Email { get; set; }
        
        [Required]
        [PasswordRequirementsAttribute]
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
