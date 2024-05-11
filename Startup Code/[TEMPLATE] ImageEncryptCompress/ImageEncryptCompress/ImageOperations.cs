using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

///Algorithms Project
///Intelligent Scissors
///

namespace ImageEncryptCompress
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
    }

    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        /// 

        public static string seedValue;
        public static int seedKey;
        public static long global_red_bytes, global_green_bytes, global_blue_bytes;

        // path for text file of the huffman tree and some other data
        //public static string CompressionPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\RGB-Tree.txt";

        // path for binary file
        public static string BinaryWriterPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\Binary.bin";

        // paths for the encrypted and decrypted images
        public static string compressedImageDataPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\com.txt";

        // paths for the encrypted and decrypted images
        public static string EncryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Encryption\\Encrypted.bmp";
        public static string DecryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Decryption\\Decrypted.bmp";

        // paths for the compressed and decompressed images
        public static string DecompressedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Decompression\\Decompressed.bmp";

        // paths for the huffman representations for each color channel of each pixel
        //public static string CompressedRedPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\R-Tree.txt";
        //public static string CompressedGreenPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\G-Tree.txt";
        //public static string CompressedBluePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\B-Tree.txt";

        // paths for compressed binary strings for each color channel of each pixel
        //public static string CompressedRedBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\R-Binary.bin";
        //public static string CompressedGreenBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\G-Binary.bin";
        //public static string CompressedBlueBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Original Materials\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\B-Binary.bin";

        //public static int red_length = 0, green_length = 0, blue_length = 0, Tape_Position = 0;
        //public static int[] R, G, B;
        //public static float matrix_dimintion = 0;
        //public static long red_bytes = 0, green_bytes = 0, blue_bytes = 0, Total_Bits = 0;
        //public static string Initial_Seed = "";
        //public static string[] arr_red, arr_gr, arr_bl;

        //public static byte[] arr, arr1, arr2;

        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }

        // function to do XORing between two characters 
        // O(1) as it is a constant time operation
        public static char XOR(char bit_1, char bit_2) // O(1)
        {
            return (bit_1 == bit_2) ? '0' : '1'; // O(1)
        }

        // function to get the k-bit shift register for each color channel
        // O(k * 3) where k here is always 8, so O(24) = O(1)
        public static string[] GetKbitSLFSR(string initialSeed, int tapPosition, int k)
        {
            // 0 == > red, 1 ==> green, 2 ==> blue
            string[] results = new string[3]; // O(1)

            // create string builder to store the answer
            StringBuilder answer = new StringBuilder(); // O(1)
            StringBuilder seedBuilder = new StringBuilder(initialSeed); // O(1)

            // convert the initial seed to binary
            int size = initialSeed.Length; // O(1)
            char tapPositionBit; // O(1)
            char firstBit; // O(1)

            int cnt = 0; // O(1)
            for (int i = 1; i <= k * 3; i++) // O(k * 3) where k here is always 8, so O(24) = O(1)
            {

                tapPositionBit = seedBuilder[size - tapPosition - 1]; // O(1)
                firstBit = seedBuilder[0]; // O(1)
                char newBit = XOR(firstBit, tapPositionBit); // O(1)

                answer.Append(newBit); // O(1)

                if (i % k == 0) // O(1)
                {
                    results[cnt] = answer.ToString(); // O(1)
                    answer.Clear(); // O(1)
                    cnt++; // O(1)
                }

                // shifting the bits to the right
                seedBuilder.Remove(0, 1); // O(1)
                seedBuilder.Append(newBit); // O(1)

                // printing the new seed
                //Console.Write(i + 1 + " ==> ");
                //Console.WriteLine(initialSeed);
            }

            seedValue = seedBuilder.ToString(); // O(1)

            return results; // O(1)
        }

        // function to encrypt the image using the LFSR algorithm
        // O(n * m) where n is the height of the image and m is the width of the image
        public static RGBPixel[,] EncryptDecryptImage(RGBPixel[,] imageMatrix, string initialSeed, int tapPosition)
        {
            // convert the initial seed to binary
            initialSeed = BONUS_Functions.AlphanumericConvertion(initialSeed); // O(n)

            // Generate LFSR keys for encryption
            seedValue = initialSeed;  // O(1)
            seedKey = tapPosition;   // O(1)

            int Height = GetHeight(imageMatrix); // O(1)
            int Width = GetWidth(imageMatrix);  // O(1)
            string[] keys; // O(1)

            // Ensure that the dimensions of the encrypted image match the dimensions of the original image
            RGBPixel[,] encryptedImage = new RGBPixel[Height, Width]; // O(1)

            // iterate over the image matrix and encrypt each pixel
            // O(n * m) where n is the height of the image and m is the width of the image
            for (int i = 0; i < Height; i++) // O(n)
            {
                for (int j = 0; j < Width; j++) // O(m)
                {
                    // get the LFSR keys for each color channel
                    keys = GetKbitSLFSR(seedValue, tapPosition, 8); // O(1)

                    // get the RGB values of the pixel
                    byte red = imageMatrix[i, j].red; // O(1)
                    byte green = imageMatrix[i, j].green; // O(1)
                    byte blue = imageMatrix[i, j].blue; // O(1)

                    // convert the RGB values to binary strings
                    byte redBinaryByte = Convert.ToByte(keys[0], 2); // O(1)
                    byte greenBinaryByte = Convert.ToByte(keys[1], 2); // O(1)
                    byte blueBinaryByte = Convert.ToByte(keys[2], 2); // O(1)

                    byte encryptedRedByte = (byte)(red ^ redBinaryByte); // O(1)
                    byte encryptedGreenByte = (byte)(green ^ greenBinaryByte); // O(1)
                    byte encryptedBlueByte = (byte)(blue ^ blueBinaryByte); // O(1)

                    // update the encrypted image matrix with the encrypted pixel
                    encryptedImage[i, j].red = encryptedRedByte; // O(1)
                    encryptedImage[i, j].green = encryptedGreenByte; // O(1)
                    encryptedImage[i, j].blue = encryptedBlueByte;  // O(1)
                }
            }

            return encryptedImage; // O(1)
        }

        // function to export the image to a file
        // O(n * m) where n is the height of the image and m is the width of the image
        public static void SaveImage(RGBPixel[,] ImageMatrix, string FilePath)
        {
            int Height = GetHeight(ImageMatrix); // O(1)
            int Width = GetWidth(ImageMatrix); // O(1)

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb); // O(1)

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat); // O(1)
                int nWidth = 0; // O(1)
                nWidth = Width * 3; // O(1)
                int nOffset = bmd.Stride - nWidth; // O(1)
                byte* p = (byte*)bmd.Scan0; // O(1)
                // iterate over the image matrix and write the pixel values to the image
                // O(n * m) where n is the height of the image and m is the width of the image
                for (int i = 0; i < Height; i++) // O(n)
                {
                    for (int j = 0; j < Width; j++) // O(m)
                    {
                        p[2] = ImageMatrix[i, j].red; // O(1)
                        p[1] = ImageMatrix[i, j].green; // O(1)
                        p[0] = ImageMatrix[i, j].blue; // O(1)
                        p += 3;     // O(1)
                    }

                    p += nOffset; // O(1)
                }
                ImageBMP.UnlockBits(bmd); // O(1)
            }

            // save the image to the file
            ImageBMP.Save(FilePath); // O(1)
        }

        // helper function to write the huffman tree to a file
        // O(n) where n is the number of nodes in the huffman tree
        // T(n) = 2T(n/2) + O(1) => O(n)
        public static void WriteHuffmanDict(HuffmanNode root, string s, Dictionary<int, string> dict, ref long Total_Bits)
        {
            // Assign 0 to the left node and recur
            if (root.Left != null)
            {
                s += '0'; // O(1)
                //                    arr[top] = 0;
                WriteHuffmanDict(root.Left, s, dict, ref Total_Bits);

                // backtracking
                s = s.Remove(s.Length - 1); // O(1)
            }

            // Assign 1 to the right node and recur
            if (root.Right != null)
            {
                s += '1'; // O(1)
                //                    arr[top] = 1;
                WriteHuffmanDict(root.Right, s, dict, ref Total_Bits);

                // backtracking
                s = s.Remove(s.Length - 1); // O(1)
            }

            // base case: if the node is a leaf node
            // O(1)
            if (root.Left == null && root.Right == null)
            {
                dict.Add(root.Pixel, s); // O(1)

                int bittat = s.Length * root.Frequency; // O(1)
                 
                Total_Bits += bittat; // O(1)

            }
        }

        // function to compress the image using huffman encoding
        // O(n*m + clog(c)) where n is the height of the image, m is the width of the image, and c is the number of colors
        public static KeyValuePair<long,double> CompressImage(RGBPixel[,] ImageMatrix, int tapPosition, string seedValue)
        {
            // get the height and width of the image
            int Height = GetHeight(ImageMatrix); // O(1)
            int Width = GetWidth(ImageMatrix); // O(1)

            // initializing the frequency of each color bit
            int[] redFreq = new int[256]; // O(1)
            int[] blueFreq = new int[256]; // O(1)
            int[] greenFreq = new int[256]; // O(1)

            // calculate the frequency of each color bit
            // O(n * m) where n is the height of the image and m is the width of the image
            for (int i = 0; i < Height; i++)    // O(n)
            {
                for (int j = 0; j < Width; j++) // O(m)
                {
                    redFreq[ImageMatrix[i, j].red]++; // O(1)
                    blueFreq[ImageMatrix[i, j].blue]++; // O(1)
                    greenFreq[ImageMatrix[i, j].green]++; // O(1)
                }
            }

            // 3 priority queues for each color
            PriorityQueue pq_red = new PriorityQueue(); // O(1)
            PriorityQueue pq_green = new PriorityQueue(); // O(1)
            PriorityQueue pq_blue = new PriorityQueue(); // O(1)

            // iterate over the frequency arrays and insert the non-zero frequencies into the priority queue
            // O(256 * log n) = O(log(n))
            for (int i = 0; i < 256; i++) // O(256) = O(1)
            {
                // ================= RED CHANNEL =================
                if (redFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i, // O(1)
                        Frequency = redFreq[i] // O(1)
                    };
                    
                    node.Left = node.Right = null; // O(1)
                    pq_red.Push(node); // O(log n)
                }
                // ================= END RED CHANNEL =================


                // ================= BLUE CHANNEL =================
                if (blueFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i, // O(1)
                        Frequency = blueFreq[i] // O(1)
                    };
                    
                    node.Left = node.Right = null; // O(1)
                    pq_blue.Push(node); // O(log n)
                }
                // ================= END BLUE CHANNEL =================

                // ================= GREEN CHANNEL =================
                if (greenFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i, // O(1)
                        Frequency = greenFreq[i] // O(1)
                    };

                    node.Left = node.Right = null; // O(1)
                    pq_green.Push(node); // O(log n)
                }
                // ================= END GREEN CHANNEL =================
            }
            
            // construct the huffman tree for the red channel
            // O(c * log c) where c is the number of nodes in the huffman tree
            while (pq_red.Count != 1) // O(c)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_red.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_red.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_red.Push(node); // O(log c)
            }

            // construct the huffman tree for the green channel
            // O(c * log c) where c is the number of nodes in the huffman tree
            while (pq_green.Count != 1) // O(c)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_green.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_green.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_green.Push(node); // O(log c)
            }

            // construct the huffman tree for the blue channel
            // O(c * log c) where c is the number of nodes in the huffman tree
            while (pq_blue.Count != 1) // O(c)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_blue.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_blue.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_blue.Push(node); // O(log c)
            }

            // get the root node of the huffman tree for each channel
            HuffmanNode theRootNode = pq_red.Pop(); // O(log n)
            HuffmanNode theRootNode2 = pq_blue.Pop(); // O(log n) 
            HuffmanNode theRootNode3 = pq_green.Pop();  // O(log n)

            long red_total_bits = 0; // O(1)
            long green_total_bits = 0; // O(1)
            long blue_total_bits = 0; // O(1)

            // dictionaries to store the huffman representation of each color bit
            Dictionary<int, string> red_dict = new Dictionary<int, string>(); // O(1)
            Dictionary<int, string> blue_dict = new Dictionary<int, string>(); // O(1)
            Dictionary<int, string> green_dict = new Dictionary<int, string>(); // O(1)

            // stream writer to write the huffman tree to file
            //StreamWriter stream = new StreamWriter(CompressionPath);

            // write the initial seed and tap position to the file
            //stream.WriteLine("Initial Seed: " + seedValue);
            //stream.WriteLine("Tap Position: " + tapPosition);

            // write the huffman tree to a file with red channel
            //            stream.WriteLine("Red - Frequency - Huffman Representation - Total Bits");
            //WriteHuffmanDict(theRootNode, null, red_dict, ref red_total_bits, stream);
            WriteHuffmanDict(theRootNode, null, red_dict, ref red_total_bits); // O(c)

            // write the huffman tree to a file with blue channel
            //            stream.WriteLine("Blue - Frequency - Huffman Representation - Total Bits");
            //WriteHuffmanDict(theRootNode2, null, blue_dict, ref blue_total_bits, stream);
            WriteHuffmanDict(theRootNode2, null, blue_dict, ref blue_total_bits); // O(c)

            // write the huffman tree to a file with green channel
            //            stream.WriteLine("Green - Frequency - Huffman Representation - Total Bits");
            //WriteHuffmanDict(theRootNode3, null, green_dict, ref green_total_bits, stream);
            WriteHuffmanDict(theRootNode3, null, green_dict, ref green_total_bits); // O(c)

            // calculate the total bytes of the image for each channel

            // red channel
            long red_rem = (red_total_bits % 8); // O(1)
            // if there are remaining bits, add an extra byte
            // O(1)
            long red_bytes; // O(1)
            if (red_rem != 0) {
                red_total_bits += 8; // O(1)
                red_bytes = red_total_bits / 8; // O(1)
                red_total_bits -= 8; // O(1)
            }
            else {
                red_bytes = red_total_bits / 8; // O(1)
            }

            

            // green channel
            long green_rem = (green_total_bits % 8); // O(1)
            // if there are remaining bits, add an extra byte
            // O(1)
            long green_bytes; // O(1)
            if (green_rem != 0) {
                green_total_bits += 8; // O(1)
                green_bytes = green_total_bits / 8;
                green_total_bits -= 8; // O(1)
            } else {
                green_bytes = green_total_bits / 8;
            }


            // blue channel
            long blue_rem = (blue_total_bits % 8); // O(1)
            // if there are remaining bits, add an extra byte
            // O(1)
            long blue_bytes;
            if (blue_rem != 0) {
                blue_total_bits += 8; // O(1)
                blue_bytes = blue_total_bits / 8;
                blue_total_bits -= 8;
            } else {
                blue_bytes = blue_total_bits / 8;
            }
            
            // calculate the total bytes of the image
            long total_bytes = red_bytes + blue_bytes + green_bytes; // O(1)

            // write the total bytes of the image
            //stream.WriteLine("total bytes: " + total_bytes);

            // write the compression ratio of the image
            long total_bits = red_total_bits + blue_total_bits + green_total_bits; // O(1)

            // product by 24 for the 3 channels (red, green, blue) and each channel has 8 bits (1 byte)
            long image_size = Height * Width * 24; 
            double compression_ratio = (double)total_bits / image_size; // O(1)
            compression_ratio *= 100; // to get the percentage
            //stream.WriteLine("Compression Ratio: " + compression_ratio * 100 + "%");

            // close the stream writer
            //stream.Close();


            // law fy remainder ehgez bytes + 1

            // byte array to store the binary representation of the image of size total_bytes for each channel
            byte[] redBinaryRepresentationToWriteInFile = new byte[red_bytes]; // O(1)
            byte[] blueBinaryRepresentationToWriteInFile = new byte[blue_bytes]; // O(1)
            byte[] greenBinaryRepresentationToWriteInFile = new byte[green_bytes]; // O(1)

            // variables to store the remaining bits in the byte
            int byte_remainder1 = 8; //O(1)
            int byte_remainder2 = 8; //O(1)
            int byte_remainder3 = 8; //O(1)

            // variables to store the index of the byte array
            int redIndex = 0; //O(1)
            int blueIndex = 0; //O(1)
            int greenIndex = 0; //O(1)

            // variables to store the huffman representation of the pixel
            string huffman_string, huffman_substr;
            int huffman_string_length;
            for (int i = 0; i < Height; i++) // O(n)
            {
                for (int j = 0; j < Width; j++) // O(m)
                {
                    
                    // ================== RED CHANNEL ==================
                    // get the huffman representation of the pixel
                    huffman_string = red_dict[ImageMatrix[i, j].red]; // O(1)
                    huffman_string_length = huffman_string.Length; // O(1)
                    // if the length of the huffman representation is less than the remaining bits in the byte
                    if (huffman_string_length < byte_remainder1)
                    {
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string_length;  // O(1)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2);  // O(1)
                        byte_remainder1 -= huffman_string_length;  // O(1)
                    }
                    else if (huffman_string_length == byte_remainder1)
                    {
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string_length; // O(1)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                        redIndex++; // O(1)
                        byte_remainder1 = 8; // O(1)
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder1); // O(1)
                        redBinaryRepresentationToWriteInFile[redIndex] <<= byte_remainder1; // O(1)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_substr, 2); // O(1)
                        redIndex++; // O(1)
                        huffman_string = huffman_string.Substring(byte_remainder1, huffman_string.Length - byte_remainder1); // O(1)

                        // iterate over the huffman representation and store the binary representation in the byte array
                        // O(n) where n is the length of the huffman representation
                        while (huffman_string.Length >= 8) 
                        {
                            huffman_substr = huffman_string.Substring(0, 8); // O(1)
                            redBinaryRepresentationToWriteInFile[redIndex] <<= 8; // O(1)
                            redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_substr, 2); // O(1)
                            redIndex++; // O(1)
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); // O(1)
                        }

                        if (huffman_string.Length != 0)
                        {
                            redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string.Length; // O(1)
                            redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                            byte_remainder1 = 8 - huffman_string.Length; // O(1)
                        }
                        else
                        {
                            byte_remainder1 = 8; // O(1)
                        }
                    }
                    // ================== END RED CHANNEL ==================

                    // ================== BLUE CHANNEL ==================
                    huffman_string = blue_dict[ImageMatrix[i, j].blue]; // O(1)
                    huffman_string_length = huffman_string.Length; // O(1)
                    if (huffman_string_length < byte_remainder2)
                    {
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string_length; // O(1)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                        byte_remainder2 -= huffman_string_length; // O(1)
                    }
                    else if (huffman_string_length == byte_remainder2)
                    {
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string_length; // O(1)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                        blueIndex++; // O(1)
                        byte_remainder2 = 8; // O(1)
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder2); // O(1)
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= byte_remainder2; // O(1)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_substr, 2);  // O(1)
                        blueIndex++; // O(1)
                        huffman_string = huffman_string.Substring(byte_remainder2, huffman_string_length - byte_remainder2); // O(1)

                        while (huffman_string.Length >= 8)
                        {
                            huffman_substr = huffman_string.Substring(0, 8); // O(1)
                            blueBinaryRepresentationToWriteInFile[blueIndex] <<= 8; // O(1)
                            blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_substr, 2); // O(1)
                            blueIndex++; // O(1)
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); // O(1)
                        }
                        if (huffman_string.Length != 0)
                        {
                            blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string.Length; // O(1)
                            blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                            byte_remainder2 = 8 - huffman_string.Length; // O(1)
                        }
                        else {
                            byte_remainder2 = 8; // O(1)
                        }
                    }
                    // ================== END BLUE CHANNEL ==================

                    // ================== GREEN CHANNEL =====================
                    huffman_string = green_dict[ImageMatrix[i, j].green]; // O(1)
                    huffman_string_length = huffman_string.Length; // O(1)
                    if (huffman_string_length < byte_remainder3)  
                    {
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string_length;  // O(1)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2);  // O(1)
                        byte_remainder3 -= huffman_string_length; // O(1)
                    }
                    else if (huffman_string_length == byte_remainder3)
                    {
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string_length;  // O(1)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                        greenIndex++;  // O(1)
                        byte_remainder3 = 8;  // O(1)
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder3); // O(1)
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= byte_remainder3; // O(1)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_substr, 2); // O(1)
                        greenIndex++; // O(1)
                        huffman_string = huffman_string.Substring(byte_remainder3, huffman_string_length - byte_remainder3);// O(1)

                        while (huffman_string.Length >= 8) 
                        {
                            huffman_substr = huffman_string.Substring(0, 8); // O(1)
                            greenBinaryRepresentationToWriteInFile[greenIndex] <<= 8; // O(1)
                            greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_substr, 2); // O(1)
                            greenIndex++; // O(1)
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); // O(1)
                        }

                        if (huffman_string.Length != 0) 
                        {
                            greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string.Length; // O(1)
                            greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); // O(1)
                            byte_remainder3 = 8 - huffman_string.Length; // O(1)
                        }
                        else
                        {
                            byte_remainder3 = 8; // O(1)
                        }
                    }
                    // ================== END GREEN CHANNEL ==================
                }
            }

            byte[] redFreqByteArr = new byte[1024]; // O(1)
            byte[] greenFreqByteArr = new byte[1024]; // O(1)
            byte[] blueFreqByteArr = new byte[1024]; // O(1)

            // convert the frequencies to byte array
            // O(256) = O(1)
            for (int i = 0; i < 256; i++) // O(256) = O(1)
            {
                Array.Copy(BitConverter.GetBytes(redFreq[i]), 0, redFreqByteArr, i * 4, 4); // O(1)
                Array.Copy(BitConverter.GetBytes(greenFreq[i]), 0, greenFreqByteArr, i * 4, 4); // O(1)
                Array.Copy(BitConverter.GetBytes(blueFreq[i]), 0, blueFreqByteArr, i * 4, 4); // O(1)
            }

            //FileStream ffs = new FileStream(compressedImageDataPath, FileMode.Truncate); // O(1)
            //StreamWriter ffss = new StreamWriter(ffs); // O(1)

            global_red_bytes = red_bytes; // O(1)
            global_green_bytes = green_bytes; // O(1)
            global_blue_bytes = blue_bytes; // O(1)

            //ffss.WriteLine(red_bytes); // O(1)
            //ffss.WriteLine(green_bytes); // O(1)
            //ffss.WriteLine(blue_bytes); // O(1)

            //ffss.WriteLine(red_rem); // O(1)
            //ffss.WriteLine(green_rem); // O(1)
            //ffss.WriteLine(blue_rem); // O(1)

            //ffss.Close(); // O(1)
            //ffs.Close(); // O(1)

            FileStream ss = new FileStream(BinaryWriterPath, FileMode.Truncate); // O(1)
            BinaryWriter binWriter = new BinaryWriter(ss); // O(1)


            binWriter.Write(redFreqByteArr); // O(1)
            binWriter.Write(greenFreqByteArr); // O(1)
            binWriter.Write(blueFreqByteArr); // O(1)

            binWriter.Write(redBinaryRepresentationToWriteInFile); // O(1)
            binWriter.Write(greenBinaryRepresentationToWriteInFile); // O(1)
            binWriter.Write(blueBinaryRepresentationToWriteInFile); // O(1)

            binWriter.Write(seedValue); // O(1)
            binWriter.Write(tapPosition); // O(1)

            binWriter.Write(Width); // O(1)
            binWriter.Write(Height); // O(1)

            binWriter.Close(); // O(1)
            ss.Close(); // O(1)

            KeyValuePair<long, double> result = new KeyValuePair<long, double>(total_bytes, compression_ratio); // O(1)

            return result; // O(1)
        }

        // function to decompress the image using the huffman tree and the binary file
        // O(n * m) where n is the height of the image and m is the width of the image
        public static RGBPixel[,] DecompressImage()
        {
            // declare the binary file stream and the binary reader
            //FileStream readingStream = new FileStream(compressedImageDataPath, FileMode.Open); // O(1)
            //StreamReader stream_reader = new StreamReader(readingStream); // O(1)

            // stream carries:
            // rgb length (3 lines)
            // binary carries:
            // (1) rgb frequencies (each one 1024 byte)
            // (2) huffman representations (3) seed (4) tap position (5) width (6) height

            // lengths of rgb bytes
            //int red_length = Convert.ToInt32(stream_reader.ReadLine()); // O(1)
            //int green_length = Convert.ToInt32(stream_reader.ReadLine()); // O(1)
            //int blue_length = Convert.ToInt32(stream_reader.ReadLine()); // O(1)

            int red_length = (int)global_red_bytes; // O(1)
            int green_length = (int)global_green_bytes; // O(1)
            int blue_length = (int)global_blue_bytes; // O(1)

            //int red_extra_bits = Convert.ToInt32(stream_reader.ReadLine());
            //int green_extra_bits = Convert.ToInt32(stream_reader.ReadLine());
            //int blue_extra_bits = Convert.ToInt32(stream_reader.ReadLine());

            //stream_reader.Close(); // O(1)
            //readingStream.Close(); // O(1)

            FileStream binaryReadingStream = new FileStream(BinaryWriterPath, FileMode.Open); // O(1)
            BinaryReader binary_reader = new BinaryReader(binaryReadingStream); // O(1)

            // frequency arrs for carrying freqs that are the 
            byte[] redFreqInBytes = binary_reader.ReadBytes(1024); // O(1)
            byte[] greenFreqInBytes = binary_reader.ReadBytes(1024); // O(1)
            byte[] blueFreqInBytes = binary_reader.ReadBytes(1024); // O(1)

            // rgb int arrs to store frequencies read from binary file
            int[] redFreq = new int[256]; // O(1)
            int[] greenFreq = new int[256]; // O(1)
            int[] blueFreq = new int[256]; // O(1)

            // priority queues for rgb to construct the tree
            PriorityQueue pq_red = new PriorityQueue(); // O(1)
            PriorityQueue pq_green = new PriorityQueue(); // O(1)
            PriorityQueue pq_blue = new PriorityQueue(); // O(1)

            // convert the frequencies from byte array to int array
            // O(1024) = O(1)
            for (int i = 0; i < 1024; i += 4) // O(1)
            {
                redFreq[i / 4] = BitConverter.ToInt32(redFreqInBytes, i); // O(1)
                greenFreq[i / 4] = BitConverter.ToInt32(greenFreqInBytes, i); // O(1)
                blueFreq[i / 4] = BitConverter.ToInt32(blueFreqInBytes, i); // O(1)
            }

            // iterate over the frequencies and insert the non-zero frequencies into the priority queue
            // O(256 * log n) = O(log n)
            for (int i = 0; i < 256; i++)
            {
                
                // ================= RED CHANNEL =================
                if (redFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i,
                        Frequency = redFreq[i]
                    };
                    node.Left = node.Right = null; // O(1)
                    pq_red.Push(node); // O(log n)
                }
                // ================= END RED CHANNEL =================


                // ================= GREEN CHANNEL =================
                if (greenFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i,
                        Frequency = greenFreq[i]
                    };
                    
                    node.Left = node.Right = null; // O(1)
                    pq_green.Push(node); // O(log n)
                }
                // ================= END GREEN CHANNEL =================


                // ================= BLUE CHANNEL =================
                if (blueFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode // O(1)
                    {
                        Pixel = i,
                        Frequency = blueFreq[i]
                    };
                    node.Left = node.Right = null; // O(1)
                    pq_blue.Push(node); // O(log n)
                }
                // ================= END BLUE CHANNEL =================
            }


            // huffman tree for the red channel
            // O(C * log C) where C is the number of nodes in the huffman tree
            while (pq_red.Count != 1)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_red.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_red.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_red.Push(node); // O(log c)
            }

            // huffman tree for the green channel
            // O(C * log C) where C is the number of nodes in the huffman tree
            while (pq_green.Count != 1)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_green.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_green.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_green.Push(node); // O(log c)
            }

            // huffman tree for the blue channel
            // O(C * log C) where C is the number of nodes in the huffman tree
            while (pq_blue.Count != 1)
            {
                HuffmanNode node = new HuffmanNode(); // O(1)
                HuffmanNode smallFreq = pq_blue.Pop(); // O(log c)
                HuffmanNode largeFreq = pq_blue.Pop(); // O(log c)

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency; // O(1)
                node.Left = largeFreq; // O(1)
                node.Right = smallFreq; // O(1)
                pq_blue.Push(node);     // O(log c)
            }

            // extract the roots of the rgb trees
            HuffmanNode rootNodeRed = pq_red.Pop(); // theta(1)
            HuffmanNode rootNodeGreen = pq_green.Pop(); // theta(1)
            HuffmanNode rootNodeBlue = pq_blue.Pop(); // theta(1)

            //read from the file the red, green, blue bytes compressed values
            byte[] compressed_red = binary_reader.ReadBytes(red_length); // O(1)
            byte[] compressed_green = binary_reader.ReadBytes(green_length); // O(1)
            byte[] compressed_blue = binary_reader.ReadBytes(blue_length); // O(1)

            seedValue = binary_reader.ReadString(); // O(1)
            seedKey = binary_reader.ReadInt32(); // O(1)

            int Width = binary_reader.ReadInt32(); // O(1)
            int Height = binary_reader.ReadInt32(); // O(1)

            binary_reader.Close(); // O(1)
            binaryReadingStream.Close(); // O(1)
            //Tape_Position = tap_position;//o(1) save it to he global value
            //Initial_Seed = seed;//o(1) save it to the global value            

            // 3 lists to save colors values that would end in 3*(N^2) space
            List<int> redPixels = new List<int>();  // O(1)
            List<int> greenPixels = new List<int>();  // O(1)
            List<int> bluePixels = new List<int>();  // O(1)

            byte byteValue = 128; // O(1)
            // 1st iter: 1000 0000, 2nd: 0100 0000, 3rd: 0010 0000, 4th: 0001 0000, 5th: 0000 1000, 6th: 0000 0100
            int currBitCount = 0; // O(1)
            HuffmanNode rootNode1 = rootNodeRed; // O(1)

            int cnt = 0; // O(1)
            long crl = compressed_red.Length; // O(1)

            // iterate over the compressed array and decompress it
            // O(n) where n is the length of the compressed array which is equal to number of pixels in image O(N*M)
            while (cnt < crl)
            {
                while (currBitCount < 8) // O(1)
                {
                    byte hettaOS = (byte)(compressed_red[cnt] & byteValue);  // O(1)
                    HuffmanNode tempNode; // O(1)
                    if (hettaOS == 0)
                    {
                        tempNode = rootNode1.Left; // O(1)
                    }
                    else { 
                        tempNode = rootNode1.Right; // O(1)
                    }

                    if (tempNode.Left == null && tempNode.Right == null) 
                    {
                        redPixels.Add(tempNode.Pixel); // O(1)
                        rootNode1 = rootNodeRed; // O(1)
                    }
                    else
                    {
                        rootNode1 = tempNode; // O(1)
                    }

                    // divide to compare with the bit on the right 
                    byteValue /= 2; // O(1)
                    // increment counter to go compare the bit to the right
                    currBitCount++;  // O(1)
                }

                cnt++; // O(1)
                currBitCount = 0; // O(1)
                byteValue = 128;  // O(1)
            }

            byteValue = 128; // O(1)
            currBitCount = 0; // O(1)
            rootNode1 = rootNodeGreen; // O(1)
            cnt = 0; // O(1)
            long cgl = compressed_green.Length; // O(1)

            // iterate over the compressed array and decompress it
            // O(n) where n is the length of the compressed array which is equal to number of pixels in image O(N*M)
            while (cnt < cgl)
            {
                while (currBitCount < 8) // O(1)
                {
                    byte hettaOS = (byte)(compressed_green[cnt] & byteValue);  // O(1)
                    HuffmanNode tempNode; // O(1)
                    if (hettaOS == 0)
                    {
                        tempNode = rootNode1.Left; // O(1)
                    }
                    else { 
                        tempNode = rootNode1.Right; // O(1)
                    }

                    if (tempNode.Left == null && tempNode.Right == null) 
                    {
                        greenPixels.Add(tempNode.Pixel); // O(1)
                        rootNode1 = rootNodeGreen; // O(1)
                    }
                    else
                    {
                        rootNode1 = tempNode; // O(1)
                    }

                    // divide to compare with the bit on the right 
                    byteValue /= 2; // O(1)
                    // increment counter to go compare the bit to the right
                    currBitCount++; // O(1)

                }
                cnt++; // O(1)
                currBitCount = 0; // O(1)
                byteValue = 128; // O(1)
            }

            byteValue = 128; // O(1)
            currBitCount = 0; // O(1)
            rootNode1 = rootNodeBlue; // O(1)
            cnt = 0; // O(1)
            long cbl = compressed_blue.Length; // O(1)

            // iterate over the compressed array and decompress it
            // O(n) where n is the length of the compressed array which is equal to number of pixels in image O(Width*Height)
            while (cnt < cbl) 
            {
                while (currBitCount < 8) // O(1)
                {
                    byte hettaOS = (byte)(compressed_blue[cnt] & byteValue); // O(1)
                    HuffmanNode tempNode; // O(1)
                    if (hettaOS == 0)
                    {
                        tempNode = rootNode1.Left; // O(1)
                    }
                    else { 
                        tempNode = rootNode1.Right; // O(1)
                    }

                    if (tempNode.Left == null && tempNode.Right == null) 
                    {
                        bluePixels.Add(tempNode.Pixel); // O(1)
                        rootNode1 = rootNodeBlue;  // O(1)
                    }
                    else
                    {
                        rootNode1 = tempNode; // O(1)
                    }

                    // divide to compare with the bit on the right 
                    byteValue /= 2; // O(1)
                    // increment counter to go compare the bit to the right
                    currBitCount++; // O(1)
                }

                cnt++; // O(1)
                currBitCount = 0; // O(1)
                byteValue = 128; // O(1)
            }

            RGBPixel[,] decompressedPicture = new RGBPixel[Height, Width]; // O(1)
            int index = 0; // O(1)

            int redLength = redPixels.Count; // O(1)
            int greenLength = greenPixels.Count; // O(1)
            int blueLength = bluePixels.Count; // O(1)

            // iterate over the decompressed pixels and store them in the decompressed picture
            // O(n * m) where n is the height of the image and m is the width of the image
            for (int i = 0; i < Height; i++) // O(n)
            {
                for (int j = 0; j < Width; j++) // O(m)
                {
                    if (index < redLength && index < greenLength && index < blueLength)
                    {
                        decompressedPicture[i, j].red = (byte)redPixels[index]; // O(1)
                        decompressedPicture[i, j].green = (byte)greenPixels[index]; // O(1)
                        decompressedPicture[i, j].blue = (byte)bluePixels[index]; // O(1)
                    }
                    index++; // O(1)
                }
            }

            return decompressedPicture; // O(1)
        }
    }
}
