using FitnessApp.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Models
{
    public class Person : IdentityUser
    {
        public DateTime RegisterDate { get; set; } // changed Once when the user register
        public DateTime LoginDate { get; set; } // changed everytime when the user logged in
        public Gender Gender { get; set; }
        public string? UserAvatar { get; set; }
        public string Role { get; set; }
    }

}
