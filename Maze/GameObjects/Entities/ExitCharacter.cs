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
        public ExitCharacter(
            Pair<Pair<int, int>, Pair<int, int>> coordinates,
            Pair<int, int> blockPos
            ):
            base(coordinates, blockPos)
        {}
        public override int drawObject(Pair<Pair<int, int>, Pair<int, int>> visibleField, ref PictureBox pictureBox, int blockSize)
        {
            try
            {
                
                Bitmap bitmap = new Bitmap(pictureBox.Image);
                Graphics g = Graphics.FromImage(bitmap);
                Brush end_brush = new SolidBrush(Color.Black);
                int radius = blockSize/4;

                g.FillEllipse(end_brush, new Rectangle(
                    (blockPos.second) * blockSize + blockSize / 2 - radius,
                    (blockPos.first) * blockSize + blockSize / 2 - radius,
                    radius * 2,
                    radius * 2
                ));

                pictureBox.Image = bitmap;
                return 0;
            }
            catch(Exception ex)
            {
                return ex.GetHashCode();
            }
        }

        public override int moveTo(ref MazeGenerator maze, Pair<int, int> pos)
        {
            return 0;
        }
    }
}
