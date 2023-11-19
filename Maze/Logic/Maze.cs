using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography;
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
            return $"{canGoTop}|{canGoRight}|{canGoBottom}|{canGoLeft}";
        }
    }

    [Serializable]
    public class MazeGenerator
    {
        private static Pair<int, int>[] directions = {
            new Pair<int, int>(0,-1),
            new Pair<int, int>(-1,0),
            new Pair<int, int>(0,1),
            new Pair<int, int>(1,0),
        };
        private static Random rand = new Random();

        private List<Pair<int, int>>[] leveled;
        private Pair<int, int> entryPoint;
        private Pair<int, int> endPoint;
        private  Cell[,]? maze;

        public Pair<int, int> Start  { get { return entryPoint; } }
        public Pair<int, int> Finish { get { return endPoint; } }
        
        public Cell this[int row,int column]
        {
            get
            {
                if (row < 0 || maze.GetLength(0) <= row) return null;
                if (column < 0 || maze.GetLength(1) <= column) return null;
                return maze[row, column];
            }
        }

        public Cell this[Pair<int, int> index] 
        {
            get
            {
                return this[index.first, index.second];
            }
        }
        public Pair<int,int> Size { get { return new(maze.GetLength(0),maze.GetLength(1)); } }
        public MazeGenerator()
        {
            entryPoint = new Pair<int, int>(0, 0);
        }
        public void generateMaze(int rows, int columns)
        {
            maze = new Cell[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    maze[r, c] = new Cell();
                }
            }

            //generate entry Point
            entryPoint = new Pair<int, int>(rand.Next(0, rows), rand.Next(0, columns));

            Pair<int, int> current_point = entryPoint;
            while(current_point.first != -1)
            {
                walk(current_point.first, current_point.second);
                current_point = getUnvisited();
                if (current_point.first == -1) break;
                connectTwoBranches(current_point.first, current_point.second);
            }

            //level the maze in order to set the exit point to one of the farthest points
            leveled = this.levelMaze(entryPoint);
            endPoint = leveled[leveled.Length - 1][rand.Next(0, leveled[leveled.Length - 1].Count)];

        }

        private Pair<int,int> getUnvisited()
        {
            List<Pair<int, int>> points = new List<Pair<int, int>>();
            for (int r = 0; r < maze.GetLength(0); r++)
            {
                for (int c = 0; c < maze.GetLength(1); c++)
                {
                    if (maze[r, c].visited) continue;
                    if (!haveVisitedNeighbours(r, c)) continue;
                    points.Add(new Pair<int, int>(r, c));
                }
            }

            return points.Count == 0 ? new Pair<int, int>(-1,-1) : points[rand.Next(0,points.Count)];
        }

        private bool connectTwoBranches(int row,int column)
        {
            List<int> canGoTo = new List<int>();
            for(int i = 0;i < 4; ++i)
            {
                int nr = row + directions[i].first;
                int nc = column + directions[i].second;
                if(
                    0 <= nr && nr < maze.GetLength(0) &&
                    0 <= nc && nc < maze.GetLength(1) &&
                    maze[nr,nc].visited
                )
                {
                    canGoTo.Add(i);
                }
            }
            if (canGoTo.Count == 0) return false;
            switch(canGoTo[rand.Next(0, canGoTo.Count)])
            {
                //go Left
                case 0:
                    maze[row, column].canGoLeft = true;
                    maze[row, column - 1].canGoRight = true;
                    break;
                //go Top
                case 1:
                    maze[row, column].canGoTop = true;
                    maze[row - 1, column].canGoBottom = true;
                    break;
                //go Right
                case 2:
                    maze[row, column].canGoRight = true;
                    maze[row, column + 1].canGoLeft = true;
                    break;
                //go Bottom
                case 3:
                    maze[row, column].canGoBottom = true;
                    maze[row + 1, column].canGoTop = true;
                    break;
            }
            return true;
        }

        private bool haveVisitedNeighbours(int row,int column)
        {
            if (row != 0 && maze[row - 1, column].visited) return true;
            if (column != 0 && maze[row, column - 1].visited) return true;
            if (row + 1 != maze.GetLength(0) && maze[row + 1, column].visited) return true;
            if( column + 1 != maze.GetLength(1) && maze[row,column + 1].visited) return true;
            return false;
        }
        private bool haveUnvisitedNeighbours(int row,int column)
        {
            return
                !(row + 1 >= maze.GetLength(0)    ? true : maze[row + 1, column].visited) ||
                !(row == 0                        ? true : maze[row - 1, column].visited) ||
                !(column + 1 >= maze.GetLength(1) ? true : maze[row, column + 1].visited) ||
                !(column == 0                     ? true : maze[row, column - 1].visited)
            ;
        }
        private void walk(int row,int column)
        {
            Queue<int> direction = new Queue<int>(30);
            int prev = -1;
            int prev_prev = -1;
            //fill the queue
            for(int i = 0;i < 30;)
            {
                int next = rand.Next(0, 201) % 4;
                if (prev_prev == next && prev == next) continue;
                (prev_prev, prev) = (prev, next);
                direction.Enqueue(next);
                ++i;
            }

            //walk untill there is no way
            while(haveUnvisitedNeighbours(row,column))
            {
                int to_skip = rand.Next(0, 10);
                while (--to_skip > 0) direction.Enqueue(direction.Dequeue());
                while (true) {
                    bool canGo = false;
                    switch (direction.Peek())
                    {
                        //go Left
                        case 0:
                            if (column == 0) break;
                            if (maze[row, column - 1].visited) break;
                            maze[row, column - 1].canGoRight = true;
                            maze[row, column].canGoLeft = true;
                            canGo = true;
                            break;
                        //go Top
                        case 1:
                            if(row == 0) break;
                            if (maze[row - 1, column].visited) break;
                            maze[row - 1, column].canGoBottom = true;
                            maze[row, column].canGoTop = true;
                            canGo = true;
                            break;
                        //go Right
                        case 2:
                            if (column + 1 == maze.GetLength(1)) break;
                            if (maze[row,column + 1].visited) break;
                            maze[row, column + 1].canGoLeft = true;
                            maze[row, column].canGoRight = true;
                            canGo = true;
                        break;
                        //go Bottom
                        case 3:
                            if (row + 1 == maze.GetLength(0)) break;
                            if (maze[row + 1, column].visited) break;
                            maze[row + 1, column].canGoTop = true;
                            maze[row, column].canGoBottom = true;
                            canGo = true;
                            break;
                    }
                    if (canGo) break;
                    direction.Enqueue(direction.Dequeue());
                }
                (prev_prev, prev) = (prev,direction.Peek());
                maze[row, column].visited = true;
                switch (direction.Peek())
                {
                    //go Left
                    case 0:
                        --column;
                        break;
                    //go Top
                    case 1:
                        --row;
                        break;
                    //go Right
                    case 2:
                        ++column;
                        break;
                    //go Bottom
                    case 3:
                        ++row;
                        break;
                }
                
            }
            maze[row, column].visited = true;
        }

        private List<Pair<int, int>>[] levelMaze(Pair<int,int> from)
        {
            Dictionary<Pair<int, int>, int> levels = new Dictionary<Pair<int, int>, int>();
            Queue<Pair<int, int>> queue = new Queue<Pair<int, int>>();
            
            queue.Enqueue(from);

            int max_level = -1;
            while(queue.Count() != 0)
            {
                ++max_level;
                int size = queue.Count();
                while(size-- > 0)
                {
                    var top = queue.Dequeue();
                    if (levels.ContainsKey(top)) continue;
                    levels[top] = max_level;
                    if (maze[top.first, top.second].canGoLeft) queue.Enqueue(new Pair<int, int>(top.first, top.second - 1));
                    if (maze[top.first, top.second].canGoTop) queue.Enqueue(new Pair<int, int>(top.first - 1, top.second));
                    if (maze[top.first, top.second].canGoRight) queue.Enqueue(new Pair<int, int>(top.first, top.second + 1));
                    if (maze[top.first, top.second].canGoBottom) queue.Enqueue(new Pair<int, int>(top.first + 1, top.second));
                }
            }

            List<Pair<int,int>>[] result = new List<Pair<int, int>>[max_level];
            
            foreach (var pair in levels)
            {
                if (result[pair.Value] == null) result[pair.Value] = new();
                result[pair.Value].Add(pair.Key);
            }
            return result;
        }
    }
}
