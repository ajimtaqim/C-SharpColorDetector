using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.ColorReduction;

namespace ColorDetector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            tableLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            pictureBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            pictureBox2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            pictureBox3.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            panel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            button2.Enabled = false;
        }

        Bitmap img = null;

        private Boolean colorIsPresent(Bitmap carImg, String color)
        {
            ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());
            ResizeBilinear rsF = new ResizeBilinear(Convert.ToInt32(carImg.Width * 0.3), Convert.ToInt32(carImg.Height * 0.3));
            pictureBox2.Image = rsF.Apply(carImg);
            Bitmap img = ciq.ReduceColors(rsF.Apply(carImg), 8);
            pictureBox3.Image = img;

            int lowVal = 0;
            int highVal = 360;
            double s = 0.15;
            double lowL = 0.05;
            double highL = 0.95;
            Boolean flag = false;
            if (!(String.Equals(color, "white", StringComparison.OrdinalIgnoreCase)) && !(String.Equals(color, "black", StringComparison.OrdinalIgnoreCase)) && !(String.Equals(color, "gray", StringComparison.OrdinalIgnoreCase)))
            {
                if (String.Equals(color, "red", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 0;
                    highVal = 20;
                }
                if (String.Equals(color, "orange", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 11;
                    highVal = 50;
                }
                if (String.Equals(color, "brown", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 21;
                    highVal = 40;
                }
                if (String.Equals(color, "yellow", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 41;
                    highVal = 80;
                }
                if (String.Equals(color, "green", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 61;
                    highVal = 169;
                }
                if (String.Equals(color, "cyan", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 141;
                    highVal = 220;
                }
                if (String.Equals(color, "blue", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 201;
                    highVal = 280;
                }
                if (String.Equals(color, "purple", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 241;
                    highVal = 330;
                }
                if (String.Equals(color, "pink", StringComparison.OrdinalIgnoreCase))
                {
                    lowVal = 321;
                    highVal = 355;
                }

                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).GetHue() >= lowVal && img.GetPixel(i, j).GetHue() <= highVal && img.GetPixel(i, j).GetSaturation() > s && img.GetPixel(i, j).GetBrightness() >= lowL && img.GetPixel(i, j).GetBrightness() <= highL)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                        break;
                }
            }
            else if (String.Equals(color, "black", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).GetBrightness() <= 0.15)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                        break;
                }
            }
            else if (String.Equals(color, "white", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).GetBrightness() >= 0.85)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                        break;
                }
            }
            else if (String.Equals(color, "gray", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).R.Equals((img.GetPixel(i, j).G).Equals(img.GetPixel(i, j).B)))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                        break;
                }
            }


            return flag;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(of.FileName);
                pictureBox1.Image = img;
                

                button2.Enabled = true;
                button1.Text = "Next Image";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorIsPresent(img, comboBox1.SelectedText) == true)
            {
                label5.Text = "Found";
            }
            else
                label5.Text = "Not found";
        }
    }
}
