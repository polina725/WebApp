using System;

namespace API.Extensions
{
    public static class DateTimeExctension
    {
        public static int CalcAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age))
                age--;
            return age--;
        }
    }
}