using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        private Button firstClicked, secondClicked;
        private Random random = new Random();
        private List<string> icons = new List<string>();
        private Timer gameTimer;
        private int elapsedTimeInSeconds;
        private bool gameStarted = false;   



        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            CreateGameGrid();
            AssignIcons();
        }

        private void CreateGameGrid()
        {
            int gridSize = 4;
            int buttonSize = 75;
            this.ClientSize = new Size(gridSize * buttonSize, gridSize * buttonSize);

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button btn = new Button();
                    btn.Width = btn.Height = buttonSize;
                    btn.Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
                    btn.Location = new Point(j * buttonSize, i * buttonSize);
                    btn.Click += Button_Click;
                    btn.BackColor = Color.LightGray;
                    this.Controls.Add(btn);
                    this.Text = ("Memory Game");
                }
            }
        }

        private void AssignIcons()
        {
            //string[] letters = { "O", "Q", "C", "S", "E", "F", "G", "Z" };
            string[] letters = { "🤣", "👍", "😍", "❤️", "😘", "😁", "😎", "😉" };
            icons = letters.Concat(letters).OrderBy(x => random.Next()).ToList();

            int i = 0;
            foreach (Control c in this.Controls)
            {
                if (c is Button btn)
                {
                    btn.Tag = icons[i];
                    btn.Text = "";
                    i++;
                }
            }
        }
        private void StartGameTimer()
        {
            elapsedTimeInSeconds = 0;
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += (s, e) =>
            {
                elapsedTimeInSeconds++;
                this.Text = $"Memory Game - Time: {elapsedTimeInSeconds} sec";
            };
            gameTimer.Start();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (!(sender is Button clickedButton)) return;

            if (clickedButton.Text != "") return;

            if (!gameStarted)
            {
                StartGameTimer();
                gameStarted = true;
            }

            if (firstClicked != null && secondClicked != null)
                return;

            clickedButton.Text = clickedButton.Tag.ToString();

            if (firstClicked == null)
            {
                firstClicked = clickedButton;
                return;
            }

            secondClicked = clickedButton;

            if (firstClicked.Tag.ToString() == secondClicked.Tag.ToString())
            {
                firstClicked = null;
                secondClicked = null;

                if (AllMatched())
                {
                    gameTimer.Stop();
                    MessageBox.Show($"You matched all pairs! \nTime: {elapsedTimeInSeconds} seconds", "Memory Game");
                    AssignIcons();
                    ResetButtons();
                    gameStarted = false;
                }
            }
            else
            {
                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    firstClicked.Text = "";
                    secondClicked.Text = "";
                    firstClicked = null;
                    secondClicked = null;
                };
                timer.Start();
            }
        }

        private void ResetButtons()
        {
            int i = 0;
            foreach (Control c in this.Controls)
            {
                if (c is Button btn)
                {
                    btn.Tag = icons[i];
                    btn.Text = "";
                    i++;
                }
            }

            elapsedTimeInSeconds = 0;
            this.Text = "Memory Game";
        }

        private bool AllMatched()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button btn && btn.Text == "")
                    return false;
            }
            return true;
        }
    }
}
