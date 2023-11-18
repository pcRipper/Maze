using Maze.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze.GameObjects.Entities
{
    public class Character : Entity
    {
        private int radius;
        private int moveSpeed;
        public Character(
            Pair<Pair<int, int>, Pair<int, int>> coordinates,
            Pair<int, int> blockPos,
            int radius,
            int moveSpeed
            ) :
            base(coordinates, blockPos)
        {
            this.radius = radius;
            this.moveSpeed = moveSpeed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibleField"></param>
        /// <param name="pictureBox"></param>
        /// <returns></returns>
        public override int drawObject(Pair<Pair<int, int>, Pair<int, int>> visibleField, ref PictureBox pictureBox,int blockSize)
        {
            try
            {

                Bitmap bitmap = new Bitmap(pictureBox.Image);
                Graphics g = Graphics.FromImage(bitmap);
                Brush character_brush = new SolidBrush(Color.Purple);

                //g.FillEllipse(character_brush, new Rectangle(pixelPos.second, pixelPos.first, radius * 2, radius * 2));

                pictureBox.Image = bitmap;

                return 0;
            }
            catch (Exception e)
            {
                return e.GetHashCode();
            }
        }

        public override int moveTo(ref MazeGenerator maze, Pair<int, int> pos)
        {
            return 0;
        }

    }
}
