using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureMazze.Console
{
    public class PictureHandler
    {
        public Bitmap BitMapToBW(Bitmap bitmap)
        {
            int rgb = 0;
            Color cl;
            Bitmap Copy = (Bitmap)bitmap.Clone();

            for(int i = 0; i < bitmap.Height; ++i)
                for(int j = 0; j < bitmap.Width; ++j)
                {
                    cl = bitmap.GetPixel(j, i);
                    rgb = (int)Math.Round(.299 * cl.R + .587 * cl.G + .114 * cl.B);
                    Copy.SetPixel(j, i, Color.FromArgb(rgb, rgb, rgb));
                }
            return Copy;
        }

        public int[][] GetMatrixFromBitMap(Bitmap bitmap)
        {
            int h = bitmap.Height;
            int w = bitmap.Width;

            int[][] m = Enumerable.Range(0, h)
                .Select(x => new int[w])
                .ToArray();
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                {
                    Color cl = bitmap.GetPixel(j, i);
                    m[i][j] = cl.R * cl.G * cl.B == 255 * 255 * 255 ? 0 : 1; 
                }
            return m;
        }

        public Bitmap DrawPoints(Bitmap bitmap, int[][] points, Color cl)
        {
            Bitmap cp = (Bitmap)bitmap.Clone();
            for (int i = 0; i < points.Length; ++i)
                cp.SetPixel(points[i][1], points[i][0], cl);
            return cp;
        }
    }
}
