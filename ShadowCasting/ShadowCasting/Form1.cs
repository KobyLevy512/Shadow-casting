using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShadowCasting
{
    public partial class Form1 : Form
    {
        const int BlockSize = 5;
        const int LightLength = 30;
        Color[] map;
        Timer paintTimer = new Timer();
        Point mouseLoc;
        Point lightLoc;
        int mapW , mapH;

        public Form1()
        {
            InitializeComponent();
        }

        private Color[] AddLight(int x, int y)
        {
            Color[] ret = new Color[map.Length];
            map.CopyTo(ret, 0);
            for(double i = 0; i< 6.282; i+= 0.01745)
            {
                double jmpX = Math.Cos(i);
                double jmpY = Math.Sin(i);
                for(int j = 0; j< LightLength; j++)
                {
                    int tmpy = (int)(lightLoc.Y + jmpY * j);
                    int tmpx = (int)(lightLoc.X + jmpX * j);
                    int pos = tmpx + tmpy * mapW;
                    if (pos < 0 || pos >= ret.Length) break;
                    if (ret[pos] != Color.Blue)
                    {
                        byte col = (byte)(255 - (j * 1.5) * BlockSize);
                        ret[pos] = Color.FromArgb(col, col, col);
                    }
                    else break;
                }
            }
            return ret;
        }
        private void Form1_Load(object sender, System.EventArgs e)
        {
            mapW = Width / BlockSize;
            mapH = Height / BlockSize;
            map = new Color[mapH * mapW];
            lightLoc = new Point(mapW / 2, mapH / 2);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Color[] map = AddLight(lightLoc.X, lightLoc.Y);
            for (int y = 0; y < mapH; y++)
            {
                for(int x =0; x< mapW; x++)
                {
                    int i = x + y * mapW;
                    if (map[i] != Color.Empty)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(map[i]), x * BlockSize, y * BlockSize, BlockSize, BlockSize);
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            paintTimer = new Timer();
            paintTimer.Interval = 20;
            paintTimer.Tick += T_Tick;
            paintTimer.Start();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            paintTimer.Stop();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLoc = e.Location;
            lightLoc = new Point(mouseLoc.X / BlockSize, mouseLoc.Y / BlockSize);
        }

        private void GameLoop_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

        //User paint loop
        private void T_Tick(object sender, System.EventArgs e)
        {
            map[(mouseLoc.X / BlockSize) + (mouseLoc.Y / BlockSize * mapW)] = Color.Blue;
            map[(mouseLoc.X / BlockSize + 1) + (mouseLoc.Y / BlockSize * mapW)] = Color.Blue;
            map[(mouseLoc.X / BlockSize) + ((mouseLoc.Y  / BlockSize+1) * mapW)] = Color.Blue;
            map[(mouseLoc.X / BlockSize + 1) + ((mouseLoc.Y  / BlockSize+1) * mapW)] = Color.Blue;
        }
    }
}
