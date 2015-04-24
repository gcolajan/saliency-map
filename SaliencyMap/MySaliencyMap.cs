using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaliencyMap
{
    public class MySaliencyMap : ISaliencyMap
    {
        const float COEFF_CENTER = 1f;
        const float COEFF_FIRST = 0.5f;
        const float COEFF_SECOND = 0.25f;

        protected int actualWidth;
        protected int actualHeight;
        protected int precision;
        protected int nbPoints;

        protected int width;
        protected int height;
        protected int[][] squares;
        protected int[][] cat;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySaliencyMap"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="areaSize">Size of the area.</param>
        public MySaliencyMap(int originalWidth, int originalHeight, int areaSize)
        {
            actualWidth = originalWidth;
            actualHeight = originalHeight;
            precision = areaSize;
            nbPoints = 0;

            // If areaSize is not ajusted with the dimension of the source, last border can't have good results
            width = actualWidth / areaSize;
            height = actualHeight / areaSize;

            if (actualWidth % areaSize > 0)
                width += 1;

            if (actualHeight % areaSize > 0)
                height += 1;

            squares = new int[width][];
            for (int i = 0; i < squares.Length; i++)
                squares[i] = new int[height];

            cat = new int[width][];
            for (int i = 0; i < cat.Length; i++)
                cat[i] = new int[height];
        }


        public void AddPoint(int x, int y)
        {
            squares[x / precision][y / precision]++;
            nbPoints++;
        }

        public void Build()
        {
            // TODO: insertion sort
            List<int> flattened = new List<int>();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    flattened.Add(squares[x][y]);
            flattened.Sort();

            int firstQuartile = flattened[flattened.Count/4];
            int mediane = flattened[flattened.Count/2];
            int lastQuartile = flattened[(3*flattened.Count)/4];

            int value;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    value = squares[x][y];
                    if (value <= firstQuartile)
                        cat[x][y] = 0;
                    else if (value <= mediane)
                        cat[x][y] = 1;
                    else if (value <= lastQuartile)
                        cat[x][y] = 2;
                    else
                        cat[x][y] = 3;
                }
            }
        }

        public Bitmap GetGraphic()
        {
            Color[] colors = { Color.FromArgb(50, 0, 0, 0), Color.FromArgb(25, 0, 0, 0), Color.FromArgb(50, 0, 0, 0), Color.FromArgb(100, 0, 0, 0) };

            Bitmap bitmap = new Bitmap(width, height);
            Bitmap result = new Bitmap(actualWidth, actualHeight);

            // Setting pixel on the small bitmap
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    bitmap.SetPixel(x, y, colors[cat[x][y]]);

            // Resize the map to the original size
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.DrawImage(bitmap, 0, 0, actualWidth, actualHeight);
            }
            return result;
        }
    }
}
