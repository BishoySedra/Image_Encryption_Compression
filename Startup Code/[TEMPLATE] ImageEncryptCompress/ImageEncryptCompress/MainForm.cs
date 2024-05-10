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
            else {
                MessageBox.Show("Please Load Image First!");
                return;
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

            // label for the compression status
            label14.Text = "";

            // label for the compression time in seconds
            label16.Text = "";

            // label for the compression time in minutes
            label17.Text = "";

            // label for the total bytes
            label20.Text = "";

            // label for the compression ratio
            label21.Text = "";

            // label for the decompression status
            label15.Text = "";

            // label for the decompression time in seconds
            label19.Text = "";

            // label for the decompression time in minutes
            label18.Text = "";

            // clear picture box 2
            pictureBox2.Image = null;
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

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            // export the image to the desktop
            ImageOperations.SaveImage(ImageMatrix, ImageOperations.EncryptedImagePath);
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

            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            // export the image to the desktop
            ImageOperations.SaveImage(ImageMatrix, ImageOperations.DecryptedImagePath);
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

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

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

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

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

            // label for the compression status
            label14.Text = "";

            // label for the compression time in seconds
            label16.Text = "";

            // label for the compression time in minutes
            label17.Text = "";

            // label for the total bytes
            label20.Text = "";

            // label for the compression ratio
            label21.Text = "";

            // label for the decompression status
            label15.Text = "";

            // label for the decompression time in seconds
            label19.Text = "";

            // label for the decompression time in minutes
            label18.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // getting the initial seed and tap position


            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // create stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            // construct the huffman tree function
            KeyValuePair<long,double> CompressionResult = ImageOperations.CompressImage(ImageMatrix, tapPosition, initialSeed);

            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            MessageBox.Show("Time: " + sw.ElapsedMilliseconds + " ms");

            long total_bytes = CompressionResult.Key;
            double compression_ratio = CompressionResult.Value;

            // show messsage box for the total bytes and the compression ratio
            MessageBox.Show("Total Bytes: " + total_bytes + "\nCompression Ratio: " + compression_ratio + "%");
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
            string input_numberOfBits = textBox2.Text;

            // check if the number of bits is not empty
            if (input_numberOfBits == "")
            {
                MessageBox.Show("Please Enter Number of Bits!");
                return;
            }

            int numberOfBits = int.Parse(input_numberOfBits);

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
            ImageOperations.SaveImage(decompressedImage, ImageOperations.DecompressedImagePath);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // getting the initial seed and tap position
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // create stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            // first encrypt the image
            ImageMatrix = ImageOperations.EncryptDecryptImage(ImageMatrix, initialSeed, tapPosition);

            // second compress the image
            KeyValuePair<long, double> CompressionResult = ImageOperations.CompressImage(ImageMatrix, tapPosition, initialSeed);

            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            long time = sw.ElapsedMilliseconds;
            long time_in_seconds = time / 1000;
            long time_in_minutes = time_in_seconds / 60;

            long total_bytes = CompressionResult.Key;
            double compression_ratio = CompressionResult.Value;

            // change the label for the compression status
            label14.Text = "Encryption and Compression Time:";
            label16.Text = "Seconds: " + time_in_seconds + " (" + time + " ms)";
            label17.Text = "Minutes: " + time_in_minutes;
            label20.Text = "Total Bytes: " + total_bytes;
            label21.Text = "Compression Ratio: " + compression_ratio + "%";
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            // getting the initial seed and tap position
            string initialSeed = txtGaussSigma.Text;
            int tapPosition = int.Parse(textBox1.Text);

            // create stop watch to calculate the time of the operation
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            // start the stop watch
            sw.Start();

            // first decompress the image
            RGBPixel[,] decompressedImage = ImageOperations.DecompressImage();

            // second decrypt the image
            ImageMatrix = ImageOperations.EncryptDecryptImage(decompressedImage, initialSeed, tapPosition);

            // stop the stop watch
            sw.Stop();

            // show the time of the operation in the message box
            long time = sw.ElapsedMilliseconds;
            long time_in_seconds = time / 1000;
            long time_in_minutes = time_in_seconds / 60;

            // change the label for the compression status
            label15.Text = "Decompression and Decryption Time:";
            label19.Text = "Seconds: " + time_in_seconds + " (" + time + " ms)";
            label18.Text = "Minutes: " + time_in_minutes;
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}