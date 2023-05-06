using FitnessApp.Enums;
using FitnessApp.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.DTOs
{
    public class InsertAdminDTO
    {
        public string Email { get; set; }
        [Required]
        [PasswordRequirementsAttribute]
        public string  Password { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
