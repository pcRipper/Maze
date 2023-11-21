using Maze.Logic;
using Maze.MDI;
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
    public partial class Menu : Form, iMdiChild
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
            this.customizeParent(this.getTopMDIParent());
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
                this.customizeParent(this.getTopMDIParent());
                this.Show();
            };

            render.customizeParent(this.getTopMDIParent());
            render.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        public void customizeParent(Form mdiParent)
        {
            if (mdiParent == null) return;
            mdiParent.MaximumSize =
            mdiParent.MinimumSize =
            mdiParent.Size = new(this.Width + 20, this.Height + 43);
            mdiParent.Text = this.Text;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
