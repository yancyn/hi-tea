using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class Utils
    {
        /// <summary>
        /// Rounding method as smallest unit 5 cents.
        /// TODO: Move to helper class.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static decimal Rounding(decimal original)
        {
            decimal result = 0m;
            decimal rounded = Math.Round(original, 1);
            decimal half = (rounded > original) ? rounded - 0.05m : rounded + 0.05m;

            decimal diff1 = rounded - original;
            if (diff1 < 0) diff1 = diff1 * -1;

            decimal diff2 = half - original;
            if (diff2 < 0) diff2 = diff2 * -1;

            result = (diff1 > diff2) ? half : rounded;

            //System.Diagnostics.Debug.WriteLine(original + " => " + rounded);
            //System.Diagnostics.Debug.WriteLine(diff1 + " <> " + diff2);
            return result;
        }
    }
}
