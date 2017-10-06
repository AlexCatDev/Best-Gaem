using System;
using System.Windows.Forms;

namespace Gaem
{
    public partial class Form1 : Form
    {
        Game game;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            game?.Quit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            game = new Game(this);
            game.Init();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            game.GameArea = this.ClientRectangle;
        }
    }
}
