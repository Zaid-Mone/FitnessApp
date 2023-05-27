using System.Text;
using System;
using System.Linq;

namespace FitnessApp.Utility
{
    public class GenerateRandomPassword
    {
        public static string RandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            int passwordLength = 6;

            StringBuilder passwordBuilder = new StringBuilder();
            Random random = new Random();

            // Add at least one lowercase character
            passwordBuilder.Append(validChars[random.Next(26)]);

            // Add at least one uppercase character
            passwordBuilder.Append(char.ToUpper(validChars[random.Next(26)]));

            // Add at least one number
            passwordBuilder.Append(validChars[random.Next(52, 62)]);

            // Add one special character
            passwordBuilder.Append(validChars[random.Next(62, 72)]);

            // Fill the remaining characters randomly
            for (int i = 4; i < passwordLength; i++)
            {
                passwordBuilder.Append(validChars[random.Next(validChars.Length)]);
            }

            // Shuffle the characters to make the password random
            string password = new string(passwordBuilder.ToString().ToCharArray().OrderBy(c => random.Next()).ToArray());

            return password;
        }

    }
}
