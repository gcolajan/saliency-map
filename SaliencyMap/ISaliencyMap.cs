using System.Drawing;

namespace SaliencyMap
{
    public interface ISaliencyMap
    {
        void AddPoint(int x, int y);
        void Build();
        Bitmap GetGraphic(int colors);
    }
}