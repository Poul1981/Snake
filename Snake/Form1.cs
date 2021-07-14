using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ;
        private PictureBox fruit;
        private int dirX, dirY;
        private readonly int width = 600;
        private readonly int height = 500;
        private readonly int sizeOfSide = 40;
        private PictureBox[] snake = new PictureBox[100];
        private int score = 0;

        public Form1()
        {
            Text = "Snake";
            ClientSize = new Size(900, 600);
            InitializeComponent();
            //KeyEventHandler key;
            //key = OKP;
            //this.KeyDown += (sender, args)=> MyKeyDown(sender, args);
            this.Width = width;
            this.Height = height;
            GenerateMap();
            GenerateFruit();
            timer.Tick += Update;
            //timer.Tick += (a, b) => Update();
            timer.Interval = 200;
            timer.Start();
            KeyDown += new KeyEventHandler(OKP);
            //KeyDown += (a, b) => OKP(a,b);
            snake[0] = new PictureBox
            {
                Location = new Point(200, 200),
                Size = new Size(sizeOfSide - 1, sizeOfSide - 1),
                //BackColor = Color.DarkGreen
                Image = Image.FromFile(@"snake_head.jpg"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            Controls.Add(snake[0]);
        }

        private void MoveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + sizeOfSide * dirX,
                                  snake[0].Location.Y + sizeOfSide * dirY);
        }

        private void EatFruit()
        {
            label1.Text = "Счет: " + ++score;
            this.Controls.Remove(fruit);

            //новый элемент змеи
            snake[score] = new PictureBox
            {
                Location = new Point(snake[score - 1].Location.X - sizeOfSide * dirX,
                                      snake[score - 1].Location.Y - sizeOfSide * dirY),
                Size = new Size(sizeOfSide - 1, sizeOfSide - 1),
                //BackColor = Color.DarkGreen
                Image = Image.FromFile(@"snake_next.jpg"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            Controls.Add(snake[score]);
            GenerateFruit();
        }

        private void GenerateFruit()
        {
            fruit = new PictureBox();
            fruit.Image = Image.FromFile(@"stroub.jpg");
            fruit.SizeMode = PictureBoxSizeMode.StretchImage;
            //fruit.BackColor = Color.Violet;
            fruit.Location = new Point(80, 80);
            fruit.Size = new Size(sizeOfSide - 1, sizeOfSide - 1);
            Random rnd = new Random();
            //отсутствие коллизий с телом змеи
            bool collision = true;
            if (score > 0)
            {
                while (collision)
                {
                    rI = rnd.Next(0, width - 100 - sizeOfSide);
                    int temp = rI % sizeOfSide;
                    rI -= temp;
                    rJ = rnd.Next(0, height - sizeOfSide);
                    temp = rJ % sizeOfSide;
                    rJ -= temp;
                    fruit.Location = new Point(rI, rJ);
                    for (int i = score - 1; i >= 0; i--)
                    {
                        if (snake[i].Location == fruit.Location)
                        {
                            collision = true;
                            break;
                        }
                        else collision = false;
                    }
                }
            }
            Controls.Add(fruit);
        }

        private void Update(object sender, EventArgs e)
        {
            MoveSnake();
            //если змея ест фрукт
            if (snake[0].Location == fruit.Location) EatFruit();

            //при выходе за пределы карты конец игры
            if (snake[0].Location.X < 0 || snake[0].Location.X > width - 40 ||
            snake[0].Location.Y < 0 || snake[0].Location.Y > height - 40) EndGame();

            //если змея кусает себя
            for (int i = 1; i <= score; i++)
            {
                if (snake[0].Location == snake[i].Location) EndGame();
            }
        }

        private void EndGame()
        {
            timer.Stop();
            DialogResult chek = MessageBox.Show("Играть заново?", "ПРОИГРЫШ :(",
            MessageBoxButtons.OKCancel);
            if (chek == DialogResult.Cancel) Application.Exit();
            if (chek == DialogResult.OK) Application.Restart();
        }

        private void GenerateMap()
        {
            //горизонтальные линии -------
            for (int i = 0; i <= height / sizeOfSide; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, sizeOfSide * i);
                pic.Size = new Size(width, 1);
                this.Controls.Add(pic);
            }
            //вертикальные линии |||||
            for (int i = 0; i <= (width) / sizeOfSide; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(sizeOfSide * i, 0);
                pic.Size = new Size(1, height - 20);
                Controls.Add(pic);
            }
        }
        //управление с клавы
        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    dirX = 1; dirY = 0;
                    break;
                case Keys.Left:
                    dirX = -1; dirY = 0;
                    break;
                case Keys.Up:
                    dirX = 0; dirY = -1;
                    break;
                case Keys.Down:
                    dirX = 0; dirY = 1;
                    break;
            }
        }
    }
}
