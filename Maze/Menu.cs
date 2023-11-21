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
using System.Xml.Serialization;

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
            try
            {
                int rows = Convert.ToInt32(numericUpDown1.Value);
                int columns = Convert.ToInt32(numericUpDown2.Value);

                if (rows < 2 || rows > 100) throw new Exception("Rows amount value should be in range[2,100]");
                if (columns < 2 || columns > 100) throw new Exception("Columns amount value should be in range[2,100]");

                MazeGenerator maze = new MazeGenerator();
                maze.generateMaze(rows, columns);
                startGame(maze);

            }catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }

        private void startGame(MazeGenerator maze)
        {
            if (maze == null) return;
            MazeRender render = new MazeRender(ref maze);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //Load maze from XML
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    if (dialog.ShowDialog() != DialogResult.OK) throw new Exception("failed to get a file");
                    
                    XmlSerializer xml = new XmlSerializer(typeof(MazeGenerator));
                    FileStream fs = new FileStream(dialog.FileName,FileMode.Open, FileAccess.Read);

                    var result = xml.Deserialize(fs);
                    if (result == null) throw new Exception("failed to deserialize the object");
                    MazeGenerator maze = (MazeGenerator)result;
                    if (maze == null) throw new Exception("failed to deserialize the object");
                    startGame(maze);
                }
            } catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
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
