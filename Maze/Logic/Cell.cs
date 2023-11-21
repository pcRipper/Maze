using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Logic
{
    [Serializable]
    public class Cell
    {
        public bool canGoLeft;
        public bool canGoRight;
        public bool canGoTop;
        public bool canGoBottom;
        public bool visited;

        public Cell()
        {
            canGoLeft = false;
            canGoRight = false;
            canGoTop = false;
            canGoBottom = false;
            visited = false;
        }

        public override string ToString()
        {
            return $"({canGoTop},{canGoRight},{canGoBottom},{canGoLeft})";
        }
    }
}
