using FitnessApp.DTOs;
using System;

namespace FitnessApp.Utility
{
    public class CalculateWeight
    {
        public static bool IsOverweight(WeightCalculateDTO person)
        {
            double bmi = person.Weight / Math.Pow(person.Height / 100, 2);
            return bmi >= 25.0;
        }

        public static int GetPerfectWeight(WeightCalculateDTO person)
        {
            double heightInMeters = person.Height / 100.0;
            double perfectBmi = 22.0;
            double perfectWeight = perfectBmi * Math.Pow(heightInMeters, 2);
            return (int)perfectWeight;
        }

        public static double GetBMI(double height, double weight)
        {
            double bmi = weight / Math.Pow(height / 100.0, 2);
            return bmi;
        }

    }
}



