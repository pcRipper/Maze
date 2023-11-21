namespace Maze
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button_exit = new Button();
            button4 = new Button();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(68, 28);
            button1.Name = "button1";
            button1.Size = new Size(100, 35);
            button1.TabIndex = 0;
            button1.Text = "Generate";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button_exit
            // 
            button_exit.Location = new Point(68, 139);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(100, 35);
            button_exit.TabIndex = 3;
            button_exit.Text = "Exit";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // button4
            // 
            button4.Location = new Point(68, 98);
            button4.Name = "button4";
            button4.Size = new Size(100, 35);
            button4.TabIndex = 4;
            button4.Text = "Load";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button2_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(68, 69);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.RightToLeft = RightToLeft.Yes;
            numericUpDown1.Size = new Size(46, 23);
            numericUpDown1.TabIndex = 5;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(122, 69);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(46, 23);
            numericUpDown2.TabIndex = 6;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(241, 209);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(button4);
            Controls.Add(button_exit);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(241, 209);
            MinimumSize = new Size(241, 209);
            Name = "Menu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Menu";
            FormClosing += Menu_FormClosing;
            Load += Menu_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button_exit;
        private Button button4;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
    }
}