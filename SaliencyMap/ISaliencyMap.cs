using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaliencyMap
{
    public interface ISaliencyMap
    {
        void AddPoint(int x, int y);
        void Build();
        Bitmap GetGraphic();
    }
}
