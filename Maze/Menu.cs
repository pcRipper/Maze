using Maze.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class Menu : Form
    {
        string previous_parent_text = String.Empty;
        public Menu()
        {
            InitializeComponent();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            if (this.MdiParent != null)
            {
                previous_parent_text = this.MdiParent.Text;
                this.MdiParent.Text = "Menu";
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.MdiParent != null)
            {
                this.MdiParent.Text = previous_parent_text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createMaze(5, 5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createMaze(10, 10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createMaze(15, 15);
        }

        private void createMaze(int rows, int columns)
        {
            MazeGenerator generator = new MazeGenerator();
            generator.generateMaze(rows, columns);
            MazeRender render = new MazeRender(ref generator);
            render.MdiParent = this.getTopMDIParent();

            this.Hide();

            render.FormClosed += (sender, e) =>
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Show();
            };

            render.StartPosition = FormStartPosition.CenterScreen;
            render.Show();
        }


    }
}
