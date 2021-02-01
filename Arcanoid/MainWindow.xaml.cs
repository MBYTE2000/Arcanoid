using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arcanoid
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        bool goRight;
        bool goLeft;
        int lives = 3;
        int speed = 10;
        int ballx = 5;
        int bally = 5;
        int score = 0;
        Random rnd = new Random();
        List<Rectangle> del = new List<Rectangle>();
        Rect ballHitBox;
        Rect playerHitBox;


        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += GameEngine;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            setColor();
            Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && Canvas.GetLeft(player) > 0)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right && Canvas.GetLeft(player) + player.Width < 800)
            {
                goRight = true;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
        }

        private void GameEngine(object sender, EventArgs e)
        {
            scoreLbl.Content = score.ToString();
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            ballHitBox = new Rect(Canvas.GetLeft(ball), Canvas.GetTop(ball), ball.Width, ball.Height);

            Canvas.SetLeft(ball, Canvas.GetLeft(ball) + ballx);
            Canvas.SetTop(ball, Canvas.GetTop(ball) - bally);


            if (goLeft)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player)-speed);
            }
            if (goRight)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
            }

            if (Canvas.GetLeft(player) < 1)
            {
                goLeft = false;
            }
            else if (Canvas.GetLeft(player) + player.Width > 800)
            {
                goRight = false;
            }

            if (Canvas.GetLeft(ball) + ball.Width > this.Width || Canvas.GetLeft(ball) < 0)
            {
                ballx = -ballx;
            }
            if (Canvas.GetTop(ball) < 0 || ballHitBox.IntersectsWith(playerHitBox))
            {
                bally = -bally;
                Canvas.SetTop(ball, Canvas.GetTop(ball) - bally - 5);
            }

            if (Canvas.GetTop(ball) + ball.Height > this.Height)
            {
                lives--;
                goRight = false;
                goLeft = false;
                Canvas.SetLeft(ball, 374);
                Canvas.SetTop(ball, 396);
                Canvas.SetLeft(player, 307);
                Canvas.SetTop(player, 432);
            }
            livesLbl.Content = lives;
            if (lives <= 0)
            {
                GameOver();
            }

            foreach (var x in GameField.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "block")
                {
                    Rect xHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (ballHitBox.IntersectsWith(xHitBox))
                    {
                        del.Add(x);
                        bally = -bally;
                        score++;
                    }
                }
            }

            if (score > 5)
            {
                ballx += 2;
                bally += 2;
            }

            if (GameField.Children.OfType<Rectangle>().Count() < 3)
            {
                GameOver();
            }

            foreach (Rectangle x in del)
            {
                GameField.Children.Remove(x);
            }

            
        }
        private void setColor()
        {
            foreach (var x in GameField.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "block")
                {
                    SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(255, (byte)rnd.Next(256), (byte)rnd.Next(256)));
                    x.Fill = brush;
                }
            }
        }
        private void Start()
        {
            goRight = false;
            goLeft = false;
            speed = 10;
            ballx = 5;
            bally = 5;
            score = 0;
            lives = 3;
            timer.Start();
        }
        private void GameOver()
        {
            timer.Stop();
            MessageBox.Show("game over");
        }
    }
}
