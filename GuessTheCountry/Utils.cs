using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheCountry
{
    public static class Utils
    {
        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.NextDouble() * (maximum - minimum) + minimum;
        }


        //public static async Task<bool> OnTimerTick()
        //{
        //    TimeSpan counter = new TimeSpan(0, 0, 3);
        //    counter -= TimeSpan.FromSeconds(1);    
        //    return true;
        //}
    }
}
