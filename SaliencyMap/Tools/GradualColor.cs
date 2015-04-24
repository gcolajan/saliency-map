using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaliencyMap.Tools
{
    public static class GradualColor
    {
        /// <summary>
        /// Require a percentage [0;1]
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <returns>Returns a Color between Blue (low percentage) and Red (high percentage).</returns>
        public static Color Get(float percentage)
        {
            if (percentage < 0f || percentage > 1f)
                throw new Exception("GradualColor.Get accept only [0f;1f] percentages.");

            int R, G, B;
            int indice = (int) (percentage * 1023);

            if (indice < 256)
            {
                R = 0;
                G = indice;
                B = 255;
            }
            else if (indice < 512)
            {
                R = 0;
                G = 255;
                B = 511 - indice;
            }
            else if (indice < 768)
            {
                R = -(512 - indice);
                G = 255;
                B = 0;
            }
            else
            {
                R = 255;
                G = 1023 - indice;
                B = 0;
            }

            Console.WriteLine(indice + " " + R + " " + G + " " + B);
            
            return Color.FromArgb(R, G, B);
        }
    }
}
