using FitnessApp.Enums;

namespace FitnessApp.Utility
{
    public class WeightState
    {
        public static string SetWeightStatus(double height,double weight)
        {
            var result = CalculateWeight.GetBMI(height, weight);

            if (result < 18.5)
            {
                return WeightStatus.Underweight.ToString();
            }
            else if (result >=18.15 && result <= 24.9)
            {
                return WeightStatus.Healthyweight.ToString();
            }
            else if (result >=25.0 && result <= 29.9)
            {
                return WeightStatus.Overweight.ToString();
            }
            else
            {
                return WeightStatus.Obesity.ToString();
            }
        }




    }
}
