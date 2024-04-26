using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

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
        public static string[] keys = new string[3];
        public static string CompressionPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Another Clone\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\RGB-Tree.txt";

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
        public static char XOR(char bit_1, char bit_2) // O(1)
        {
            return (bit_1 == bit_2) ? '0' : '1';
        }

        // function to get the k-bit shift register for each color channel
        // O(k * 3) where k here is always 8, so O(24) = O(1)
        public static string[] getKbitSLFSR(string initialSeed, int tapPosition, int k)
        {
            // 0 == > red, 1 ==> green, 2 ==> blue
            string[] results = new string[3];
            string answer = String.Empty;

            // first getting the tap position bit and the first bit will be shifted
            int size = initialSeed.Length;
            char tapPositionBit;
            char firstBit;

            int cnt = 0;
            for (int i = 1; i <= k * 3; i++)
            {

                tapPositionBit = initialSeed[size - tapPosition - 1];
                firstBit = initialSeed[0];
                char newBit = XOR(firstBit, tapPositionBit);

                answer += newBit;

                if (i % k == 0)
                {
                    results[cnt] = answer;
                    answer = String.Empty;
                    cnt++;
                }

                // shifting the bits to the right
                initialSeed = (initialSeed.Substring(1) + newBit);

                // printing the new seed
                Console.Write(i + 1 + " ==> ");
                Console.WriteLine(initialSeed);
            }

            seedValue = initialSeed;

            return results;
        }

        // function to convert from byte to integer
        public static int ByteToInt(byte b) // O(1)
        {
            return Convert.ToInt32(b);
        }

        // function to convert from integer to binary string consisting of 8 bits 
        public static string convertToBinary(int num) // O(1)
        {
            return Convert.ToString(num, 2).PadLeft(8, '0');
        }

        // function to do XORing between two binary strings
        // O(n) where n is the length of the binary string where n here is always 8, so O(8) = O(1)
        public static string XOR(string a, string b)
        {
            string result = String.Empty;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    result += "0";
                }
                else
                {
                    result += "1";
                }
            }
            return result;
        }

        // function to convert from binary string to byte
        public static byte convertToByte(string binary) // O(1)
        {
            return Convert.ToByte(binary, 2);
        }

        // function to encrypt the image using the LFSR algorithm
        // O(n * m) where n is the height of the image and m is the width of the image
        public static RGBPixel[,] Encrypt(RGBPixel[,] imageMatrix, string initialSeed, int tapPosition)
        {
            // Generate LFSR keys for encryption
            seedValue = initialSeed;
            seedKey = tapPosition;

            int Height = GetHeight(imageMatrix); // O(1)
            int Width = GetWidth(imageMatrix);  // O(1)

            // Ensure that the dimensions of the encrypted image match the dimensions of the original image
            RGBPixel[,] encryptedImage = new RGBPixel[Height, Width];

            // iterate over the image matrix and encrypt each pixel
            // O(n * m) where n is the height of the image and m is the width of the image
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    // get the LFSR keys for each color channel
                    // O(1)
                    keys = getKbitSLFSR(seedValue, tapPosition, 8);

                    // get the RGB values of the pixel
                    // O(1)
                    byte red = imageMatrix[i, j].red;
                    byte green = imageMatrix[i, j].green;
                    byte blue = imageMatrix[i, j].blue;

                    // convert the RGB values to binary strings
                    // O(1)
                    string redBinary = convertToBinary(ByteToInt(red));
                    string greenBinary = convertToBinary(ByteToInt(green));
                    string blueBinary = convertToBinary(ByteToInt(blue));

                    // encrypt the RGB values using the LFSR keys
                    // O(1)
                    string encryptedRed = XOR(redBinary, keys[0]);
                    string encryptedGreen = XOR(greenBinary, keys[1]);
                    string encryptedBlue = XOR(blueBinary, keys[2]);

                    // convert the encrypted RGB values to bytes
                    // O(1)
                    byte encryptedRedByte = convertToByte(encryptedRed);
                    byte encryptedGreenByte = convertToByte(encryptedGreen);
                    byte encryptedBlueByte = convertToByte(encryptedBlue);

                    // update the encrypted image matrix with the encrypted pixel
                    // O(1)
                    encryptedImage[i, j].red = encryptedRedByte;
                    encryptedImage[i, j].green = encryptedGreenByte;
                    encryptedImage[i, j].blue = encryptedBlueByte;
                }
            }

            return encryptedImage;
        }

        // function to decrypt the image using the LFSR algorithm
        // O(n * m) where n is the height of the image and m is the width of the image
        public static RGBPixel[,] Decrypt(RGBPixel[,] imageMatrix, string InitialSeed, int tapPosition)
        {
            // recall the encrypption function to get the seed value
            // O(n * m) where n is the height of the image and m is the width of the image

            // Generate LFSR keys for decryption
            //seedValue = InitialSeed;
            //int Height = GetHeight(imageMatrix); // O(1)
            //int Width = GetWidth(imageMatrix);  // O(1)

            //// Ensure that the dimensions of the decrypted image match the dimensions of the original image
            //RGBPixel[,] decryptedImage = new RGBPixel[Height, Width];

            //// iterate over the image matrix and decrypt each pixel
            //// O(n * m) where n is the height of the image and m is the width of the image

            //for (int i = 0; i < Height; i++)
            //{
            //    for (int j = 0; j < Width; j++)
            //    {
            //        // get the LFSR keys
            //        // O(1)
            //        keys = getKbitSLFSR(seedValue, tapPosition, 8);

            //        // get the RGB values of the pixel
            //        // O(1)
            //        byte red = imageMatrix[i, j].red;
            //        byte green = imageMatrix[i, j].green;
            //        byte blue = imageMatrix[i, j].blue;

            //        // convert the RGB values to binary strings
            //        // O(1)
            //        string redBinary = convertToBinary(ByteToInt(red));
            //        string greenBinary = convertToBinary(ByteToInt(green));
            //        string blueBinary = convertToBinary(ByteToInt(blue));

            //        // decrypt the RGB values using the LFSR keys
            //        // O(1)
            //        string decryptedRed = XOR(redBinary, keys[0]);
            //        string decryptedGreen = XOR(greenBinary, keys[1]);
            //        string decryptedBlue = XOR(blueBinary, keys[2]);

            //        // convert the decrypted RGB values to bytes
            //        // O(1)
            //        byte decryptedRedByte = convertToByte(decryptedRed);
            //        byte decryptedGreenByte = convertToByte(decryptedGreen);
            //        byte decryptedBlueByte = convertToByte(decryptedBlue);

            //        // update the decrypted image matrix with the decrypted pixel
            //        // O(1)
            //        decryptedImage[i, j].red = decryptedRedByte;
            //        decryptedImage[i, j].green = decryptedGreenByte;
            //        decryptedImage[i, j].blue = decryptedBlueByte;
            //    }
            //}

            // just recall the encryption function to get the decrypted image
            // O(n * m) where n is the height of the image and m is the width of the image

            RGBPixel[,] decryptedImage = Encrypt(imageMatrix, InitialSeed, tapPosition);

            return decryptedImage;
        }

        // function to export the image to a file
        // O(n * m) where n is the height of the image and m is the width of the image
        public static void ExportImage(RGBPixel[,] ImageMatrix, string FilePath)
        {
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

            ImageBMP.Save(FilePath);
        }

        // function to test the identicality of two images
        // O(n * m) where n is the height of the image and m is the width of the image
        public static bool TestIdenticality(RGBPixel[,] ImageMatrix1, RGBPixel[,] ImageMatrix2)
        {
            int Height1 = ImageMatrix1.GetLength(0);
            int Width1 = ImageMatrix1.GetLength(1);

            int Height2 = ImageMatrix2.GetLength(0);
            int Width2 = ImageMatrix2.GetLength(1);

            if (Height1 != Height2 || Width1 != Width2)
            {
                return false;
            }

            for (int i = 0; i < Height1; i++)
            {
                for (int j = 0; j < Width1; j++)
                {
                    if (ImageMatrix1[i, j].red != ImageMatrix2[i, j].red || ImageMatrix1[i, j].green != ImageMatrix2[i, j].green || ImageMatrix1[i, j].blue != ImageMatrix2[i, j].blue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        // helper function to write the huffman tree to a file
        private static void WriteHuffmanDict(HuffmanNode root, string s, Dictionary<int, string> dict, ref long Total_Bits, StreamWriter stream)
        {
            // Assign 0 to the left node and recur
            if (root.Left != null)
            {
                s += '0';
                //                    arr[top] = 0;
                WriteHuffmanDict(root.Left, s, dict, ref Total_Bits, stream);
                // backtracking
                s = s.Remove(s.Length - 1);
            }

            // Assign 1 to the right node and recur
            if (root.Right != null)
            {
                s += '1';
                //                    arr[top] = 1;
                WriteHuffmanDict(root.Right, s, dict, ref Total_Bits, stream);
                // backtracking
                s = s.Remove(s.Length - 1);
            }

            // If this is a leaf node, then we print root.Data
            // We also print the code for this character from
            // arr
            if (root.Left == null && root.Right == null)
            {
                dict.Add(root.Pixel, s);
                //            Color - Frequency - Huffman Representation - Total Bits
                //          24 - 20299 - 0 - 20299 // 1 bit * 20299
                int bittat = s.Length * root.Frequency;

                stream.WriteLine(Convert.ToString(root.Pixel) + " - " // Color
               + Convert.ToString(root.Frequency) + " - " // Frequency
               + s + " - " // huffman representation
                           // s -> "110" => 3 bits * frequency of pixel
               + Convert.ToString(bittat)); // total bits eli by representaha el pixel de

                Total_Bits += bittat; //o(1)-(addition&& assignment)
                                      //for (int i = 0; i < top; ++i)
                                      //{
                                      //  Console.Write(arr[i]);
                                      //}
                                      //Console.WriteLine();
            }
        }

        // function to construct the huffman tree for the image
        public static KeyValuePair<long, double> ConstructHuffmanTree(RGBPixel[,] ImageMatrix)
        {

            Dictionary<int, int> redFreq = new Dictionary<int, int>();
            Dictionary<int, int> blueFreq = new Dictionary<int, int>();
            Dictionary<int, int> greenFreq = new Dictionary<int, int>();

            int Height = GetHeight(ImageMatrix);
            int Width = GetWidth(ImageMatrix);

            // initializing the colors bits 0 -> 255 with frequency 0
            for (int i = 0; i < 256; i++)
            {
                redFreq.Add(i, 0);
                blueFreq.Add(i, 0);
                greenFreq.Add(i, 0);
            }

            // calculating the frequency of each bit
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    redFreq[ImageMatrix[i, j].red]++;
                    blueFreq[ImageMatrix[i, j].blue]++;
                    greenFreq[ImageMatrix[i, j].green]++;
                }
            }

            // building the huffman tree using priority queue
            PriorityQueue pq_red = new PriorityQueue();
            PriorityQueue pq_blue = new PriorityQueue();
            PriorityQueue pq_green = new PriorityQueue();

            // construct the priority queue with the red frequency
            foreach (KeyValuePair<int, int> redPixel in redFreq)
            {
                // insert the item into the priority queue and construct
                // 256 HuffmanNode kol node shayla el pixel value (0 -> 255) w shayla el frequency bta3t el pixel de
                // w shayla (left, right) = null

                if (redPixel.Value != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = redPixel.Key,
                        Frequency = redPixel.Value
                    };
                    node.Left = node.Right = null;
                    pq_red.Push(node);
                }

            }

            // construct the priority queue with the blue frequency
            foreach (KeyValuePair<int, int> bluePixel in blueFreq)
            {
                // insert the item into the priority queue and construct
                // 256 HuffmanNode kol node shayla el pixel value (0 -> 255) w shayla el frequency bta3t el pixel de
                // w shayla (left, right) = null

                if (bluePixel.Value != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = bluePixel.Key,
                        Frequency = bluePixel.Value
                    };
                    node.Left = node.Right = null;
                    pq_blue.Push(node);
                }

            }

            // construct the priority queue with the green frequency
            foreach (KeyValuePair<int, int> greenPixel in greenFreq)
            {
                // insert the item into the priority queue and construct
                // 256 HuffmanNode kol node shayla el pixel value (0 -> 255) w shayla el frequency bta3t el pixel de
                // w shayla (left, right) = null

                if (greenPixel.Value != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = greenPixel.Key,
                        Frequency = greenPixel.Value
                    };
                    node.Left = node.Right = null;
                    pq_green.Push(node);
                }

            }

            // construct the huffman tree for the red channel
            while (pq_red.Count != 1)
            {
                HuffmanNode node = new HuffmanNode();
                HuffmanNode smallFreq = pq_red.Pop();
                HuffmanNode largeFreq = pq_red.Pop();

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency;
                node.Left = largeFreq;
                node.Right = smallFreq;
                pq_red.Push(node);
            }

            // construct the huffman tree for the blue channel
            while (pq_blue.Count != 1)
            {
                HuffmanNode node = new HuffmanNode();
                HuffmanNode smallFreq = pq_blue.Pop();
                HuffmanNode largeFreq = pq_blue.Pop();

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency;
                node.Left = largeFreq;
                node.Right = smallFreq;
                pq_blue.Push(node);
            }

            // construct the huffman tree for the green channel
            while (pq_green.Count != 1)
            {
                HuffmanNode node = new HuffmanNode();
                HuffmanNode smallFreq = pq_green.Pop();
                HuffmanNode largeFreq = pq_green.Pop();

                node.Frequency = smallFreq.Frequency + largeFreq.Frequency;
                node.Left = largeFreq;
                node.Right = smallFreq;
                pq_green.Push(node);
            }

            // get the root node of the huffman tree for each channel
            HuffmanNode theRootNode = pq_red.Pop();
            HuffmanNode theRootNode2 = pq_blue.Pop();
            HuffmanNode theRootNode3 = pq_green.Pop();

            // declare the file stream and the stream writer
            FileStream the_file_stream = new FileStream(CompressionPath, FileMode.Truncate);
            StreamWriter stream = new StreamWriter(the_file_stream);

            // write the initial seed and tap position to the file
            stream.WriteLine("Initial Seed: " + seedValue);
            stream.WriteLine("Tap Position: " + seedKey);

            stream.WriteLine("====================================");

            // write the huffman tree to a file with red channel
            stream.WriteLine("Red - Frequency - Huffman Representation - Total Bits");
            string s = null;
            long red_total_bits = 0;
            Dictionary<int, string> red_dict = new Dictionary<int, string>();
            WriteHuffmanDict(theRootNode, s, red_dict, ref red_total_bits, stream);
            red_total_bits += (red_total_bits % 8);
            long red_bytes = red_total_bits / 8;

            stream.WriteLine("====================================");

            // write the huffman tree to a file with green channel
            stream.WriteLine("Green - Frequency - Huffman Representation - Total Bits");
            string s3 = null;
            long green_total_bits = 0;
            Dictionary<int, string> green_dict = new Dictionary<int, string>();
            WriteHuffmanDict(theRootNode3, s3, green_dict, ref green_total_bits, stream);
            green_total_bits += (green_total_bits % 8);
            long green_bytes = green_total_bits / 8;

            stream.WriteLine("====================================");


            // write the huffman tree to a file with blue channel
            stream.WriteLine("Blue - Frequency - Huffman Representation - Total Bits");
            string s2 = null;
            long blue_total_bits = 0;
            Dictionary<int, string> blue_dict = new Dictionary<int, string>();
            WriteHuffmanDict(theRootNode2, s2, blue_dict, ref blue_total_bits, stream);
            blue_total_bits += (blue_total_bits % 8);
            long blue_bytes = blue_total_bits / 8;

            stream.WriteLine("====================================");


            // write the total bytes of the image
            long total_bytes = red_bytes + blue_bytes + green_bytes;
            stream.WriteLine("Total Bytes: " + total_bytes);

            // write the compression ratio of the image
            long total_bits = red_total_bits + blue_total_bits + green_total_bits;
            long image_size = Height * Width * 24; // product by 24 for the 3 channels (red, green, blue) and each channel has 8 bits (1 byte)
            double compression_ratio = (double)total_bits / image_size ;

            stream.WriteLine("Compression Ratio: " + compression_ratio * 100 + "%");

            stream.Close();

            KeyValuePair<long, double> result = new KeyValuePair<long, double>(total_bytes, compression_ratio * 100);

            return result;
        }

    }
}