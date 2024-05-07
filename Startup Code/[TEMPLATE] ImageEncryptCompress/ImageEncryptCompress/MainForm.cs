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
        RGBPixel[,] ImageMatrix2;
        RGBPixel[,] DesiredImageMatrix;

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

            // clear the text box for the number of bits
            textBox2.Text = "";

            // label for the image 1 status 
            label12.Text = "";

            // label for the image 2 status
            label13.Text = "";

            //  label for the desired image status
            label10.Text = "";

            // label for attacking status
            label9.Text = "";
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

            ImageMatrix = ImageOperations.EncryptDecryptImage(ImageMatrix, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();


            // if the time of the operation is not zero
            if (sw.ElapsedMilliseconds == 0)
            {
                // show the time of the operation in the message box
                MessageBox.Show("Already Encrypted!");
            }
            else
            {
                // show the time of the operation in the message box
                MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");
            }


            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            // export the image to the desktop
            ImageOperations.ExportImage(ImageMatrix, ImageOperations.EncryptedImagePath);
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

            ImageMatrix = ImageOperations.EncryptDecryptImage(ImageMatrix, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();

            if (sw.ElapsedMilliseconds == 0)
            {
                // show the time of the operation in the message box
                MessageBox.Show("Already Decrypted!");
            }
            else
            {
                // show the time of the operation in the message box
            }
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            // export the image to the desktop
            ImageOperations.ExportImage(ImageMatrix, ImageOperations.DecryptedImagePath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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

            ImageMatrix = ImageOperations.EncryptDecryptImage(ImageMatrix, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();

            if (sw.ElapsedMilliseconds == 0)
            {
                // show the time of the operation in the message box
                MessageBox.Show("Already Encrypted!");
            }
            else
            {
                // show the time of the operation in the message box
                MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");
            }

            ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // showing message box for initial seed and tap position
            //MessageBox.Show("Initial Seed: " + initialSeed + "\nTap Position: " + tapPosition);

            // declare stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            ImageMatrix = ImageOperations.EncryptDecryptImage(ImageMatrix, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();

            if (sw.ElapsedMilliseconds == 0)
            {
                // show the time of the operation in the message box
                MessageBox.Show("Already Decrypted!");
            }
            else
            {
                // show the time of the operation in the message box
                MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");
            }

            ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                // change label12 text
                label12.Text = "Image 1 loaded Successfully!";
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog2.FileName;
                ImageMatrix2 = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix2, pictureBox2);
                // change label13 text
                label13.Text = "Image 2 loaded Successfully!";
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            // if the one of the images is not loaded
            if (ImageMatrix == null || ImageMatrix2 == null)
            {
                MessageBox.Show("Please Load Both Images First!");
                return;
            }

            // testing the identicality of the two images
            bool identical = BONUS_Functions.TestIdenticality(ImageMatrix, ImageMatrix2);

            if (identical)
            {
                MessageBox.Show("IDENTICAL!!");
                return;
            }

            MessageBox.Show("NOT IDENTICAL!!");

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // getting the initial seed and tap position
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // construct the huffman tree function
            long total_bytes = ImageOperations.CompressImage(ImageMatrix, tapPosition, initialSeed);

            // show messsage box for the total bytes
            MessageBox.Show("Total Bytes: " + total_bytes);

            // show message box for the result
            //MessageBox.Show("Compression Ratio: " + result.Value + "%\nCompression Output: " + result.Key + " bytes");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                DesiredImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
            }

            // if the image is successfully loaded
            if (DesiredImageMatrix != null)
            {
                label10.Text = "Image Loaded Successfully!";
            }

            // change attacking status text
            label9.Text = "Attacking Running...";

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

            // taking input from textBox2
            int numberOfBits = int.Parse(textBox2.Text);

            // attack the image with the desired number of bits
            Tuple<string, int> result = BONUS_Functions.Attack(ImageMatrix, DesiredImageMatrix, numberOfBits);

            // show the result in the message box
            if (result != null)
            {
                label9.Text = "Attack Successful!\n Seed Value = " + result.Item1 + "\n Tap Position = " + result.Item2;
            }
            else
            {
                label9.Text = "Attack Failed!";
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // change attacking status text
            label9.Text = "Attacking Running...";
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            

            // stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            // getting the decompressed image
            RGBPixel[,] decompressedImage = ImageOperations.DecompressImage();

            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            // display the decompressed image
            ImageOperations.DisplayImage(decompressedImage, pictureBox1);

            // export the decompressed image
            ImageOperations.ExportImage(decompressedImage, ImageOperations.DecompressedImagePath);
        }
    }
}