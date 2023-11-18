using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Maze.Logic;

namespace Maze.GameObjects.Entities
{
    /// <summary>
    /// describes an object on 2D plane, where 
    //  pixelPos -> is the position in pixels of the object's center
    //  blockPos -> pos of the block, where object is located
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// first  -> y,
        /// second -> x
        /// </summary>
        public Pair<Pair<int, int>, Pair<int, int>> coordinates;
        /// <summary>
        /// first  -> y,
        /// second -> x
        /// </summary>
        public Pair<int, int> blockPos;

        public Entity(
            Pair<Pair<int, int>, Pair<int, int>> coordinates = default,
            Pair<int, int> blockPos = default
            )
        {
            this.coordinates = new(coordinates);
            this.blockPos = new(blockPos);
        }
        /// <summary>
        /// method for drawing an object
        /// </summary>
        /// <param name="visibleField">two dots, where:
        /// first -> top right corner of visible field
        /// second -> bottom left corner of visible field    
        /// </param>
        /// <param name="pictureBox"> instance of pictureBox for drawing </param>
        /// <returns>0 - on succeess or error code</returns>
        public abstract int drawObject(Pair<Pair<int, int>,Pair<int, int>> visibleField, ref PictureBox pictureBox,int blockSize);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">move To next</param>
        /// <returns></returns>
        public abstract int moveTo(ref MazeGenerator maze, Pair<int, int> pos);
        /// <summary>
        /// check if the current object is in the zone,by block_pos
        /// </summary>
        /// <param name="zone">zone, where: 
        /// zone.first  -> top right corner, 
        /// zone.second -> bottom left corner
        /// </param>
        /// <returns>if current object is in the zone</returns>
    }
}
