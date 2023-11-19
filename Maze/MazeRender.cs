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
using System.Xml;
using Maze.GameObjects.Entities;
using Maze.Logic;

namespace Maze
{
    public partial class MazeRender : Form
    {
        static private int BLOCK_SIZE = 75;
        static private int OFFSETS = 500;
        static private int WALL_THICKNES = 3;

        private Bitmap renderedMaze;
        private int VISIBLE_HEIGHT;
        private int VISIBLE_WIDTH;

        private Dictionary<string, Entity> entities;
        private MazeGenerator maze;
        private bool ReachedEixt;
        public MazeRender(ref MazeGenerator maze)
        {
            InitializeComponent();
            this.KeyPreview = true;

            this.maze = maze;
            ReachedEixt = false;
            entities = new();
            renderedMaze = new(
                OFFSETS * 2 + BLOCK_SIZE * maze.Size.second,
                OFFSETS * 2 + BLOCK_SIZE * maze.Size.first
            );
            RenderMaze(ref renderedMaze);
            VISIBLE_HEIGHT = pictureBox1.Height;
            VISIBLE_WIDTH = pictureBox1.Width;
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
                    new(BLOCK_SIZE * maze.Finish.first + OFFSETS, BLOCK_SIZE * maze.Finish.second + OFFSETS),
                    new(BLOCK_SIZE * (maze.Finish.first + 1) + OFFSETS, BLOCK_SIZE * (maze.Finish.second + 1) + OFFSETS)
                ),
                new(maze.Finish)
            );
            int character_radius = BLOCK_SIZE / 4;
            entities["main"] = new Character(
                new(
                    new(BLOCK_SIZE * maze.Start.first + BLOCK_SIZE / 2 - character_radius + OFFSETS, BLOCK_SIZE * maze.Start.second + BLOCK_SIZE / 2 - character_radius + OFFSETS),
                    new(BLOCK_SIZE * maze.Start.first + BLOCK_SIZE / 2 + character_radius + OFFSETS, BLOCK_SIZE * maze.Start.second + BLOCK_SIZE / 2 + character_radius + OFFSETS)
                ),
                new(maze.Start),
                BLOCK_SIZE / 4,
                15
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

        private void RenderGamePicture(ref Bitmap gamePicture,Rectangle visibleRectangle)
        {
            textBox1.AppendText($"Capturing: ({visibleRectangle.Y},{visibleRectangle.X})->({visibleRectangle.Y + visibleRectangle.Height},{visibleRectangle.X + visibleRectangle.Width})");

            using (Graphics g = Graphics.FromImage(gamePicture))
            {
                g.DrawImage(renderedMaze, new Rectangle(new(0, 0), pictureBox1.Size), visibleRectangle, GraphicsUnit.Pixel);
            }
            pictureBox1.Image = gamePicture;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //set fragment of map
            Pair<int, int> center = Functions.getCenter(entities["main"].coordinates);
            Rectangle visibleRectangle = new(
                Math.Max(OFFSETS, center.second - VISIBLE_WIDTH / 2),
                Math.Max(OFFSETS, center.first - VISIBLE_HEIGHT / 2),
                VISIBLE_WIDTH,
                VISIBLE_HEIGHT
            );

            Bitmap gamePicture = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            RenderGamePicture(ref gamePicture,visibleRectangle);

            //set entitites
            foreach (var obj in entities)
            {
                obj.Value.drawObject(
                    ref gamePicture,
                    MazeRender.BLOCK_SIZE,
                    visibleRectangle
                );
            }

            pictureBox1.Image = gamePicture;
        }

        private void MazeRender_KeyPress(object sender, KeyPressEventArgs e)
        {
            Entity character = entities["main"];

            int result_code;
            switch (e.KeyChar)
            {
                case 'w':
                    result_code = entities["main"].move(ref maze, new(-1, 0), BLOCK_SIZE,OFFSETS);
                    break;
                case 'a':
                    result_code = entities["main"].move(ref maze, new(0, -1), BLOCK_SIZE, OFFSETS);
                    break;
                case 's':
                    result_code = entities["main"].move(ref maze, new(1, 0), BLOCK_SIZE, OFFSETS);
                    break;
                case 'd':
                    result_code = entities["main"].move(ref maze, new(0, 1), BLOCK_SIZE, OFFSETS);
                    break;
                default:
                    result_code = 1;
                    break;
            }

            if (result_code != 0) return;
            if (!ReachedEixt)
            {

            }

            

            button2_Click(null, null);
        }
    }
}