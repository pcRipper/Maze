using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
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
        /// 
        /// </summary>
        /// <param name="picture">picture to draw an image in</param>
        /// <param name="blockSize">size(in pixels) of a square block</param>
        /// <param name="render_start">region, that currently captured and will be represented on screen</param>
        /// <returns>status code</returns>
        public abstract int drawObject(ref Bitmap picture,int blockSize, Rectangle render_zone);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">move To next</param>
        /// <returns></returns>
        public abstract int move(ref MazeGenerator maze, Pair<int, int> vector, int blockSize,int offsets);
        
        protected bool isVisible(Rectangle renderZone)
        {
            return Functions.Collided(
                new(
                    new(renderZone.Y,renderZone.X),
                    new(renderZone.Y + renderZone.Height, renderZone.X + renderZone.Width)
                ), 
                coordinates
            );
        }
    }
}
