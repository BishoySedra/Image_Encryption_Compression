using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageEncryptCompress
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            // getting the intial seed and tap position
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // showing message box for initial seed and tap position
            //MessageBox.Show("Initial Seed: " + initialSeed + "\nTap Position: " + tapPosition);

            // declare stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            ImageMatrix = ImageOperations.Encrypt(ImageMatrix, initialSeed, tapPosition);
            
            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtGaussSigma_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // showing message box for initial seed and tap position
            //MessageBox.Show("Initial Seed: " + initialSeed + "\nTap Position: " + tapPosition);

            // declare stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            ImageMatrix = ImageOperations.Decrypt(ImageMatrix, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}