using Maze.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze.GameObjects.Entities
{
    public class Character : Entity
    {
        private static Brush character_brush = new SolidBrush(Color.Purple);
        private int moveSpeed;
        private int radius;
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
        public override int drawObject(ref Bitmap picture,int blockSize, Rectangle render_zone)
        {
            try
            {
                Graphics g = Graphics.FromImage(picture);

                g.FillEllipse(character_brush, 
                    new Rectangle(
                        coordinates.first.second - render_zone.X,
                        coordinates.first.first - render_zone.Y,
                        coordinates.second.second - coordinates.first.second,
                        coordinates.second.first - coordinates.first.first
                    )
                );
                
                return 0;
            }
            catch (Exception e)
            {
                return e.GetHashCode();
            }
        }

        public override int move(ref MazeGenerator maze, Pair<int, int> vector, int blockSize, int offsets)
        {
            int i = 0;
            while(i < moveSpeed)
            {
                Pair<Pair<int, int>, Pair<int, int>> next_pos = new(
                    new(coordinates.first.first + vector.first, coordinates.first.second + vector.second),
                    new(coordinates.second.first + vector.first, coordinates.second.second + vector.second)
                );
                Pair<int, int> block_left = new(
                    (next_pos.first.first-offsets)/blockSize,
                    (next_pos.first.second-offsets)/blockSize
                );
                Pair<int, int> block_right = new(
                    (next_pos.second.first-offsets) / blockSize,
                    (next_pos.second.second - offsets) / blockSize
                );
                if (next_pos.first.first <= offsets || next_pos.first.second <= offsets) break;
                

                if (block_left != block_right)
                {
                    if (block_left.first < block_right.first && !maze[block_left].canGoBottom) break;
                    if (block_left.first > block_right.first && !maze[block_left].canGoTop) break;
                    if (block_left.second < block_right.second && !maze[block_left].canGoRight) break;
                    if (block_left.second > block_right.second && !maze[block_left].canGoLeft) break;
                }

                coordinates = next_pos;
                i += 1;
            }

            return i > 0 ? 0 : -1;
        }

    }
}
