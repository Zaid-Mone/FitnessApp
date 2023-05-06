using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Validations
{
    public class PasswordRequirementsAttribute : RegularExpressionAttribute
    {
        public PasswordRequirementsAttribute()
            : base("^((?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z0-9])).{6,}$")
        {
            ErrorMessage = "The password must have at least 1 uppercase letter, 1 number, 1 symbol, and be at least 6 characters long.";
        }
    }
}
