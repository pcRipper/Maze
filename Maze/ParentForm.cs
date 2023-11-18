namespace Maze
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            startApplication();
        }

        private void startApplication()
        {
            Menu menu = new Menu();
            menu.MdiParent = this;
            menu.Show();
        }
    }
}