using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheCountry
{
    /// <summary>
    /// Utilities class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Get random number
        /// </summary>
        /// <param name="minimum">Minimun value for the random number</param>
        /// <param name="maximum">Maximum value for the random number</param>
        /// <returns>Random double number</returns>
        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
