using Maze.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze.GameObjects.Entities
{
    public class ExitCharacter : Entity
    {
        private static Brush end_brush = new SolidBrush(Color.Black);

        private int radius;
        public ExitCharacter(
            Pair<Pair<int, int>, Pair<int, int>> coordinates,
            Pair<int, int> blockPos,
            int radius
            ):
            base(coordinates, blockPos)
        {
            this.radius = radius;
        }
        public override int drawObject(ref Bitmap picture, int blockSize,Rectangle render_zone)
        {
            try
            {
                if (!isVisible(render_zone)) return -1;
                Graphics g = Graphics.FromImage(picture);
                g.FillEllipse(end_brush,
                    new(
                        coordinates.first.second - render_zone.X + blockSize/2 - radius,
                        coordinates.first.first - render_zone.Y + blockSize / 2 - radius,
                        radius*2,
                        radius*2
                    )
                );
                

                return 0;
            }
            catch(Exception ex)
            {
                return ex.GetHashCode();
            }
        }

        public override int move(ref MazeGenerator maze, Pair<int, int> vector, int blockSize, int offsets)
        {
            return 0;
        }
    }
}
