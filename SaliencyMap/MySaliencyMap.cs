using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SaliencyMap.Tools;

namespace SaliencyMap
{
    public class MySaliencyMap : ISaliencyMap
    {
        private const float CoeffCenter = 1f;
        private const float CoeffFirst = 0.5f;
        private const float CoeffSecond = 0.25f;
        private readonly int _actualHeight;
        private readonly int _actualWidth;
        private readonly int _precision;
        private readonly int _processHeight;
        private readonly int _processWidth;
        private List<int> _sortedOccurrences;
        private readonly int[][] _squares;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MySaliencyMap" /> class.
        /// </summary>
        /// <param name="originalWidth">The width.</param>
        /// <param name="originalHeight">The height.</param>
        /// <param name="areaSize">Size of the area.</param>
        public MySaliencyMap(int originalWidth, int originalHeight, int areaSize)
        {
            _actualWidth = originalWidth;
            _actualHeight = originalHeight;
            _precision = areaSize;

            // If areaSize is not ajusted with the dimension of the source, last border can't have good results
            _processWidth = _actualWidth/areaSize;
            _processHeight = _actualHeight/areaSize;

            if (_actualWidth%areaSize > 0)
                _processWidth += 1;

            if (_actualHeight%areaSize > 0)
                _processHeight += 1;

            _squares = new int[_processWidth][];
            for (var i = 0; i < _squares.Length; i++)
                _squares[i] = new int[_processHeight];
        }

        public void AddPoint(int x, int y)
        {
            _squares[x/_precision][y/_precision]++;
        }

        public void Build()
        {
            // TODO: insertion sort
            _sortedOccurrences = new List<int>();
            for (var x = 0; x < _processWidth; x++)
                for (var y = 0; y < _processHeight; y++)
                    _sortedOccurrences.Add(_squares[x][y]);
            _sortedOccurrences.Sort();
        }

        public Bitmap GetGraphic(int colors)
        {
            // Working size
            var bitmap = new Bitmap(_processWidth, _processHeight);

            // Original size of the picture
            var result = new Bitmap(_actualWidth, _actualHeight);

            // Setting pixel on the small bitmap
            for (var x = 0; x < _processWidth; x++)
            {
                for (var y = 0; y < _processHeight; y++)
                {
                    int q, lowerBound, upperBound;
                    var v = _squares[x][y];

                    lowerBound = 0;
                    for (q = 0; q < colors; q++)
                    {
                        upperBound = (int) (((float) (q + 1)/colors)*_sortedOccurrences.Count) - 1;
                        if (v >= _sortedOccurrences[lowerBound] && v <= _sortedOccurrences[upperBound])
                            break;

                        lowerBound = upperBound + 1;
                    }


                    bitmap.SetPixel(x, y, GradualColor.Get((float) q/colors));
                }
            }


            // Resize the map to the original size
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImage(bitmap, 0, 0, _actualWidth, _actualHeight);
            }
            return result;
        }
    }
}