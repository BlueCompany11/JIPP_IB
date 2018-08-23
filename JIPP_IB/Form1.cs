using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JIPP_IB
{
    public partial class Form1 : Form
    {
        delegate void IsGameOver();
        event IsGameOver OnIsGameOver;
        //pierwszy element to kolumna, drugi rzad
        State[,] data = new State[3, 3];
        public Form1()
        {
            InitializeComponent();
            //zaczynamy od kolka
            currentMove = State.Circle;
            //reakcja na zakonczenie gry sa te 3 metody ponizej
            //wyrazenie lambda (zeby po prostu uzyc) ktore wyswietli okienko kto wygral
            OnIsGameOver = (() => { MessageBox.Show(currentMove.ToString() + " win"); });
            //czyszczenie grafiki
            OnIsGameOver += Form1_OnIsGameOver;
            //czyszczenie modelu logicznego
            OnIsGameOver += ClearData;
        }

        private void Form1_OnIsGameOver()
        {
            ClearFields(pictureBox1);
            ClearFields(pictureBox2);
            ClearFields(pictureBox3);
            ClearFields(pictureBox4);
            ClearFields(pictureBox5);
            ClearFields(pictureBox6);
            ClearFields(pictureBox7);
            ClearFields(pictureBox8);
            ClearFields(pictureBox9);
        }
        void ClearData()
        {
            data = new State[3, 3];
        }
        private void ClearFields(PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            g.Clear(pictureBox.BackColor);
        }
        State currentMove;
        enum State
        {
            None,
            Circle,
            Cross
        }
        
        //metoda sprawdzajaca czy gra jest juz zakonczona
        bool CheckIfGameFinished()
        {
            State currentCheckVal = currentMove;
            //przeszukiwanie poziomo
            for (int i = 0; i < 3; i++)
            {
                bool res = true;
                for (int j = 0; j < 3; j++)
                {
                    if (data[i, j] != currentMove)
                    {
                        res = false;
                        break;
                    }
                }
                if (res == true)
                    return true;
            }
            //przeszuiwanie pionowo
            for (int i = 0; i < 3; i++)
            {
                bool res = true;
                for (int j = 0; j < 3; j++)
                {
                    if (data[j,i] != currentMove)
                    {
                        res = false;
                        break;
                    }
                }
                if (res == true)
                    return true;
            }
            //przeszukiwanie po ukosie
            bool diagonal1 = true;
            for (int i = 0; i < 3; i++)
            {
                if (data[i,i] != currentMove)
                {
                    diagonal1 = false;
                    break;
                }
            }
            if (diagonal1 == true)
                return true;
            //w druga strone
            bool diagonal2 = true;
            for (int i = 0; i < 3; i++)
            {
                if (data[2-i,i] != currentMove)
                {
                    diagonal2 = false;
                    break;
                }
            }
            if (diagonal2 == true)
                return true;
            if (!data.Cast<State>().Any(o => o == State.None))
            {
                currentMove = State.None;
                return true;
            }
            return false;
        }
        
        void DrawElement(PictureBox pictureBox, int posX, int posY)
        {
            if(currentMove == State.Circle)
            {
                DrawCircle(pictureBox);
            }
            else if(currentMove == State.Cross)
            {
                DrawCross(pictureBox);
            }
            else// tu wejdziemy gdy gra zakonczyla sie remisem -zalozylem, ze zaczniemy od okregow
            {
                currentMove = State.Circle;
                DrawCircle(pictureBox);
            }
            data[posX, posY] = currentMove;
            if (CheckIfGameFinished())
            {
                OnIsGameOver();
            }
            //zamiana wartosc - zawsze na koncu
            if (currentMove == State.Circle)
            {
                currentMove = State.Cross;
            }
            else if(currentMove == State.Cross)
            {
                currentMove = State.Circle;
            }
        }
        private void DrawCircle(PictureBox pictureBox)
        {
            int x = 2;
            Graphics g = pictureBox.CreateGraphics();
            int centerX = pictureBox.Width / x;
            int centerY = pictureBox.Height / x;
            int radius = pictureBox.Height / x;
            g.DrawEllipse(Pens.Red, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
            
        }
        private void DrawCross(PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            g.DrawLine(new Pen(Color.Black, 1),
                     1,
                     1,
                     pictureBox.Width,
                     pictureBox.Height);

            g.DrawLine(new Pen(Color.Black, 1),
                     pictureBox.Width,
                     1,
                     1,
                     pictureBox.Height);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox1,0,0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox2, 1, 0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox3, 2, 0);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox4, 0, 1);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox5, 1, 1);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox6, 2, 1);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox7, 0, 2);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox8, 1, 2);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            DrawElement(pictureBox9, 2, 2);
        }
    }
}
