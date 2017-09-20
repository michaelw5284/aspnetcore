using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Gets the current age
        /// </summary>
        /// <param name="dateTimeOffset">Passes offeset</param>
        /// <returns>an integer</returns>
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Year;

            if (currentDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
