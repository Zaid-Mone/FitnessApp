using FitnessApp.Enums;

namespace FitnessApp.DTOs
{
    public class InsertTrainerDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
