using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureMazze.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            PictureHandler pictureHandler = new PictureHandler();
            //I convert picture to gray, it's not necessary for this but It makes this more clear when show path
            Bitmap bitmap = pictureHandler.BitMapToBW(new Bitmap("Mazze.bmp"));
            bitmap.Save("BWMazze.bmp");
            Walker walker = new Walker();
            int[][] matrix = pictureHandler.GetMatrixFromBitMap(bitmap);
            int[][] path = walker.FindPath(matrix);
            if(path == null)
            {
                System.Console.WriteLine("No Path");
                return;
            }
            Bitmap pathOnBitmap = pictureHandler.DrawPoints(bitmap, path, Color.Red);
            pathOnBitmap.Save("MazzePath.bmp");
        }
    }


    internal class Walker
    {
        private int[][] moves = new[] 
        {
            new[] { 0, 1 },
            new[] { 1, 0 },
            new[] { 0, -1 },
            new[] { -1, 0 },
            new[] { -1, -1 },
            new[] { 1, 1 },
            new[] { -1, 1 },
            new[] { 1, -1 }
        };
        Random rnd = new Random();

        public int[][] FindPath(int[][] Matrix, int xi = 0, int yi = 0)
        {
            int mx = Matrix.Length;
            int my = Matrix[0].Length;
            Queue<int[]> qt = new Queue<int[]>();
            int[][] D = Enumerable.Range(0, Matrix.Length)
                .Select(x => Enumerable.Range(0, Matrix[0].Length)
                .Select(y => int.MaxValue)
                .ToArray())
                .ToArray();

            D[xi][yi] = 0;
            qt.Enqueue(new[] { xi, yi });
            while(qt.Count > 0)
            {
                int[] pos = qt.Dequeue();
                if (pos[0] == mx - 1 && pos[1] == my - 1)
                    break;
                for(int i = 0; i < moves.Length; ++i)
                {
                    int nx = pos[0] + moves[i][0];
                    int ny = pos[1] + moves[i][1];
                    if (!CheckBound(nx, ny, mx, my) || Matrix[nx][ny] != 0)
                        continue;
                        
                    if(D[nx][ny] > D[pos[0]][pos[1]] + 1)
                    {
                        D[nx][ny] = D[pos[0]][pos[1]] + 1;
                        qt.Enqueue(new[] { nx, ny });
                    }
                }
            }

            int xf = mx - 1;
            int yf = my - 1;
            if (D[xf][yf] == int.MaxValue)
                return null;

            List<int[]> path = new List<int[]>();
            BuildPath(mx - 1, my - 1, xi, yi, mx, my, D, path);
            return path.ToArray();
        }


        private bool BuildPath(int x, int y, int xi, int yi, int mx, int my, int[][] D, List<int[]> path)
        {
            path.Add(new[] { x, y });

            if (x == xi && y == yi)
                return true;

            //this is just to get a more smooth walk instead a linear one
            int ri = rnd.Next() % moves.Length;
            int direccion = rnd.Next() % 2 == 0 ? 1 : -1;

            for(int i = 0; i < moves.Length; i += direccion)
            {
                int ni = (i + ri + moves.Length) % moves.Length;
                int nx = x + moves[ni][0];
                int ny = y + moves[ni][1];
                if (CheckBound(nx, ny, mx, my) && D[nx][ny] == D[x][y] - 1)
                    return BuildPath(nx, ny, xi, yi, mx, my, D, path);
            }
            return false;
        }

        private bool CheckBound(int x, int y, int mx, int my)
            => 0 <= x && x < mx && 0 <= y && y < my;
    }
}
