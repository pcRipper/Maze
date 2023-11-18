using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze.GameObjects.Entities;
using Maze.Logic;

namespace Maze
{
    public partial class MazeRender : Form
    {
        static private int BLOCK_SIZE = 50;
        static private int VISIBLE_RADIUS = 4;
        static private int OFFSETS = 25;
        static private int WALL_THICKNES = 2;

        private Bitmap renderedMaze;
        private Dictionary<string, Entity> entities;
        private MazeGenerator maze;
        private bool ReachedEixt;
        private Pair<Pair<int, int>, Pair<int, int>> visible_field;
        public MazeRender(ref MazeGenerator maze)
        {
            InitializeComponent();
            this.KeyPreview = true;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            this.maze = maze;
            ReachedEixt = false;
            entities = new();
            renderedMaze = new (
                OFFSETS * 2 + BLOCK_SIZE * maze.Size.second,
                OFFSETS * 2 + BLOCK_SIZE * maze.Size.first
            );
            RenderMaze(ref renderedMaze);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MazeRender_Load(object sender, EventArgs e)
        {
            //init characters
            entities["exit"] = new ExitCharacter(
                new(
                    new(BLOCK_SIZE * maze.Finish.first, BLOCK_SIZE * maze.Finish.second),
                    new(BLOCK_SIZE * (maze.Finish.first + 1), BLOCK_SIZE * (maze.Finish.second + 1))
                ),
                new(maze.Finish)
            );
            int character_radius = BLOCK_SIZE / 4;
            entities["main"] = new Character(
                new(
                    new(BLOCK_SIZE * maze.Start.first + BLOCK_SIZE / 2 - character_radius, BLOCK_SIZE * maze.Start.second + BLOCK_SIZE / 2 - character_radius),
                    new(BLOCK_SIZE * maze.Start.first + BLOCK_SIZE / 2 + character_radius, BLOCK_SIZE * maze.Start.second + BLOCK_SIZE / 2 + character_radius)
                ),
                new(maze.Start),
                BLOCK_SIZE / 4,
                3
            );

            //set visible field
            visible_field = new(
                new(Math.Max(0, maze.Start.first - VISIBLE_RADIUS), Math.Max(0, maze.Start.second - VISIBLE_RADIUS)),
                new(Math.Min(maze.Size.first - 1, maze.Start.first + VISIBLE_RADIUS), Math.Min(maze.Size.second - 1, maze.Start.second + VISIBLE_RADIUS))
            );
        }

        

        private void RenderMaze(ref Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);
            Brush empty_brush = new SolidBrush(Color.DarkGray);
            Brush maze_background_brush = new SolidBrush(Color.Gray);
            Brush border_brush = new SolidBrush(Color.Red);

            g.FillPolygon(empty_brush, 
                new Point[] { 
                    new Point(0,0), 
                    new Point(bitmap.Width,0),
                    new Point(bitmap.Width, bitmap.Height),
                    new Point(0,bitmap.Height)
                }
            );

            g.FillPolygon(maze_background_brush,
                new Point[] { 
                    new Point(OFFSETS,OFFSETS),
                    new Point(OFFSETS + BLOCK_SIZE * maze.Size.second,OFFSETS),
                    new Point(OFFSETS + BLOCK_SIZE * maze.Size.second,OFFSETS + BLOCK_SIZE * maze.Size.first),
                    new Point(OFFSETS,OFFSETS + BLOCK_SIZE * maze.Size.first),
                }
            );

            for (int r = 0; r < maze.Size.first; ++r)
            {
                for (int c = 0; c < maze.Size.second; ++c)
                {
                    int fromR = BLOCK_SIZE * r + OFFSETS;
                    int fromC = BLOCK_SIZE * c + OFFSETS;

                    if (!maze[r, c].canGoLeft)
                    {
                        g.FillPolygon(border_brush, new Point[] {
                            new Point(fromC,fromR),
                            new Point(fromC + WALL_THICKNES,fromR),
                            new Point(fromC + WALL_THICKNES,fromR + BLOCK_SIZE),
                            new Point(fromC,fromR + BLOCK_SIZE),
                        });
                    }
                    if (!maze[r, c].canGoTop)
                    {
                        g.FillPolygon(border_brush, new Point[] {
                            new Point(fromC,fromR),
                            new Point(fromC + BLOCK_SIZE,fromR),
                            new Point(fromC + BLOCK_SIZE,fromR + WALL_THICKNES),
                            new Point(fromC,fromR + WALL_THICKNES)
                        });
                    }
                    if (!maze[r, c].canGoBottom)
                    {
                        g.FillPolygon(border_brush, new Point[] {
                            new Point(fromC,fromR + BLOCK_SIZE - WALL_THICKNES),
                            new Point(fromC + BLOCK_SIZE,fromR + BLOCK_SIZE - WALL_THICKNES),
                            new Point(fromC + BLOCK_SIZE,fromR + BLOCK_SIZE),
                            new Point(fromC,fromR + BLOCK_SIZE)
                        });
                    }
                    if (!maze[r, c].canGoRight)
                    {
                        g.FillPolygon(border_brush, new Point[] {
                            new Point(fromC + BLOCK_SIZE - WALL_THICKNES,fromR),
                            new Point(fromC + BLOCK_SIZE,fromR),
                            new Point(fromC + BLOCK_SIZE,fromR + BLOCK_SIZE),
                            new Point(fromC + BLOCK_SIZE - WALL_THICKNES,fromR + BLOCK_SIZE)
                        });
                    }
                }
            }
        }

        private void RenderGamePicture()
        {
            pictureBox1.Image = renderedMaze;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RenderGamePicture();

            foreach (var obj in entities)
            {
                obj.Value.drawObject(
                    visible_field,
                    ref pictureBox1,
                    MazeRender.BLOCK_SIZE
                );
            }
        }

        private void MazeRender_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Pair<int, int> next_pixel_top = new Pair<int, int>(pixel_pos);
            //Pair<int, int> next_pixel_bottom = new Pair<int, int>(pixel_pos.first + radius * 2, pixel_pos.second + radius * 2);
            //bool can_move = false;
            //switch (e.KeyChar)
            //{
            //    case 'w':
            //        next_pixel_top.first -= step;
            //        next_pixel_bottom.first -= step;
            //        break;
            //    case 'a':
            //        next_pixel_top.second -= step;
            //        next_pixel_bottom.second -= step;

            //        break;
            //    case 's':
            //        next_pixel_top.first += step;
            //        next_pixel_bottom.first += step;

            //        break;
            //    case 'd':
            //        next_pixel_top.second += step;
            //        next_pixel_bottom.second += step;
            //        break;
            //}
            //can_move = canGo(next_pixel_top) && canGo(next_pixel_bottom);
            //textBox1.AppendText($"{pixel_pos.first}x{pixel_pos.second}|{block_pos.first}x{block_pos.second}" + Environment.NewLine);
            //if (!can_move) return;
            //pixel_pos = next_pixel_top;
            //block_pos.first = pixel_pos.first / BLOCK_SIZE;
            //block_pos.second = pixel_pos.second / BLOCK_SIZE;

            //if (!ReachedEixt)
            //{

            //}

            button2_Click(null, null);
        }
    }
}