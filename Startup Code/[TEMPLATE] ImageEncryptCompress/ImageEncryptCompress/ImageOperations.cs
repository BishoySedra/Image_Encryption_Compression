﻿using System;
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
        public static bool isEncrypted = false;
        public static string CompressionPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\RGB-Tree.txt";
        public static string BinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\Binary.bin";
        public static string EncryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Encryption\\Encrypted.bmp";
        public static string DecryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Decryption\\Decrypted.bmp";

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
                char newBit = helpers.XOR(firstBit, tapPositionBit);

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

        // function to encrypt the image using the LFSR algorithm
        // O(n * m) where n is the height of the image and m is the width of the image
        public static RGBPixel[,] EncryptDecryptImage(RGBPixel[,] imageMatrix, string initialSeed, int tapPosition)
        {

            // convert the initial seed to binary
            initialSeed = BONUS_Functions.AlphanumericConvertion(initialSeed);

            // Generate LFSR keys for encryption
            seedValue = initialSeed;
            seedKey = tapPosition;

            int Height = GetHeight(imageMatrix); // O(1)
            int Width = GetWidth(imageMatrix);  // O(1)
            string[] keys = new string[3];

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
                    string redBinary = helpers.convertToBinary(helpers.ByteToInt(red));
                    string greenBinary = helpers.convertToBinary(helpers.ByteToInt(green));
                    string blueBinary = helpers.convertToBinary(helpers.ByteToInt(blue));

                    // encrypt the RGB values using the LFSR keys
                    // O(1)
                    string encryptedRed = helpers.XOR(redBinary, keys[0]);
                    string encryptedGreen = helpers.XOR(greenBinary, keys[1]);
                    string encryptedBlue = helpers.XOR(blueBinary, keys[2]);

                    // convert the encrypted RGB values to bytes
                    // O(1)
                    byte encryptedRedByte = helpers.convertToByte(encryptedRed);
                    byte encryptedGreenByte = helpers.convertToByte(encryptedGreen);
                    byte encryptedBlueByte = helpers.convertToByte(encryptedBlue);

                    // update the encrypted image matrix with the encrypted pixel
                    // O(1)
                    encryptedImage[i, j].red = encryptedRedByte;
                    encryptedImage[i, j].green = encryptedGreenByte;
                    encryptedImage[i, j].blue = encryptedBlueByte;
                }
            }



            return encryptedImage;
        }

        // function to export the image to a file
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

        // helper function to write the huffman tree to a file
        public static void WriteHuffmanDict(HuffmanNode root, string s, Dictionary<int, string> dict, ref long Total_Bits, StreamWriter stream)
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

                stream.WriteLine(Convert.ToString(root.Pixel) + " " // Color
               + Convert.ToString(root.Frequency) + " " // Frequency
               + s + " " // huffman representation
                         //s-> "110" => 3 bits * frequency of pixel
                            + Convert.ToString(bittat)); // total bits eli by representaha el pixel de

                Total_Bits += bittat; //o(1)-(addition&& assignment)
                                      //for (int i = 0; i < top; ++i)
                                      //{
                                      //  Console.Write(arr[i]);
                                      //}
                                      //Console.WriteLine();
            }
        }

        // function to compress the image using the huffman algorithm and write the huffman tree to a binary file
        // function to compress the image using the huffman algorithm
        public static long CompressImage(RGBPixel[,] ImageMatrix, int tapPosition, string seedValue)
        {
            // get the height and width of the image
            int Height = GetHeight(ImageMatrix);
            int Width = GetWidth(ImageMatrix);

            // initializing the frequency of each color bit
            int[] redFreq = new int[256];
            int[] blueFreq = new int[256];
            int[] greenFreq = new int[256];

            // calculate the frequency of each color bit
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    redFreq[ImageMatrix[i, j].red]++;
                    blueFreq[ImageMatrix[i, j].blue]++;
                    greenFreq[ImageMatrix[i, j].green]++;
                }
            }


            // construct the priority queue with the red frequency
            PriorityQueue pq_red = new PriorityQueue();

            // iterate over the red frequency array and insert the non-zero frequencies into the priority queue
            for (int i = 0; i < 256; i++)
            {
                if (redFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = i,
                        Frequency = redFreq[i]
                    };
                    node.Left = node.Right = null;
                    pq_red.Push(node);
                }
            }

            //construct the priority queue with the blue frequency
            PriorityQueue pq_blue = new PriorityQueue();

            // iterate over the blue frequency array and insert the non-zero frequencies into the priority queue
            for (int i = 0; i < 256; i++)
            {
                if (blueFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = i,
                        Frequency = blueFreq[i]
                    };
                    node.Left = node.Right = null;
                    pq_blue.Push(node);
                }
            }

            //construct the priority queue with the green frequency
            PriorityQueue pq_green = new PriorityQueue();

            // iterate over the green frequency array and insert the non-zero frequencies into the priority queue
            for (int i = 0; i < 256; i++)
            {
                if (greenFreq[i] != 0)
                {
                    HuffmanNode node = new HuffmanNode
                    {
                        Pixel = i,
                        Frequency = greenFreq[i]
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

            long red_total_bits = 0;
            long green_total_bits = 0;
            long blue_total_bits = 0;

            // create dictionaries to store the huffman representation of each color bit
            Dictionary<int, string> red_dict = new Dictionary<int, string>();
            Dictionary<int, string> blue_dict = new Dictionary<int, string>();
            Dictionary<int, string> green_dict = new Dictionary<int, string>();

            // create a stream writer to write the huffman tree to a file
            StreamWriter stream = new StreamWriter(CompressionPath);

            // write the initial seed and tap position to the file
            stream.WriteLine("Initial Seed: " + seedValue);
            stream.WriteLine("Tap Position: " + tapPosition);

            // write the huffman tree to a file with red channel
            stream.WriteLine("Red - Frequency - Huffman Representation - Total Bits");
            WriteHuffmanDict(theRootNode, null, red_dict, ref red_total_bits, stream);

            // write the huffman tree to a file with blue channel
            stream.WriteLine("Blue - Frequency - Huffman Representation - Total Bits");
            WriteHuffmanDict(theRootNode2, null, blue_dict, ref blue_total_bits, stream);

            // write the huffman tree to a file with green channel
            stream.WriteLine("Green - Frequency - Huffman Representation - Total Bits");
            WriteHuffmanDict(theRootNode3, null, green_dict, ref green_total_bits, stream);

            // calculate the total bytes of the image for each channel

            // red channel
            long rem = (red_total_bits % 8);
            long red_bytes = red_total_bits / 8;
            if (rem != 0) {
                red_bytes++;
            }

            // green channel
            //green_total_bits += (green_total_bits % 8);
            long green_bytes = green_total_bits / 8;
            rem = (green_total_bits % 8);
            if (rem != 0) {
                green_bytes++;
            }

            // blue channel
            //blue_total_bits += (blue_total_bits % 8);
            long blue_bytes = blue_total_bits / 8;
            rem = (blue_total_bits % 8);
            if (rem != 0)
            {
                blue_bytes++;
            }

            // calculate the total bytes of the image
            long total_bytes = red_bytes + blue_bytes + green_bytes;

            // write the total bytes of the image
            stream.WriteLine("Total Bytes: " + total_bytes);

            // write the compression ratio of the image
            long total_bits = red_total_bits + blue_total_bits + green_total_bits;
            long image_size = Height * Width * 24; // product by 24 for the 3 channels (red, green, blue) and each channel has 8 bits (1 byte)
            double compression_ratio = (double)total_bits / image_size;
            stream.WriteLine("Compression Ratio: " + compression_ratio * 100 + "%");

            // close the stream writer
            stream.Close();

            // byte array to store the binary representation of the image of size total_bytes for each channel
            byte[] redBinaryRepresentationToWriteInFile = new byte[red_bytes];
            byte[] blueBinaryRepresentationToWriteInFile = new byte[blue_bytes];
            byte[] greenBinaryRepresentationToWriteInFile = new byte[green_bytes];

            // variables to store the remaining bits in the byte
            int byte_remainder1 = 8; //O(1)-(assignment)
            int byte_remainder2 = 8; //O(1)-(assignment)
            int byte_remainder3 = 8; //O(1)-(assignment)

            // variables to store the index of the byte array
            int redIndex = 0; //o(1)-(assignment)
            int blueIndex = 0; //o(1)-(assignment)
            int greenIndex = 0; //o(1)-(assignment)

            // variables to store the huffman representation of the pixel
            string huffman_string, rf;

            for (int i = 0; i < Height; i++) //o(h*w)
            {
                for (int j = 0; j < Width; j++) //o(w)
                {
                    // red channel

                    // get the huffman representation of the pixel
                    huffman_string = red_dict[ImageMatrix[i, j].red]; // temp = huffman representation of pixel 0

                    // if the length of the huffman representation is less than the remaining bits in the byte
                    if (huffman_string.Length < byte_remainder1)
                    {
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                                                                                                  // 10101, arr[idx] = 0000 0000 => 0000 0000
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                             // 0001 0101
                        byte_remainder1 -= huffman_string.Length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string.Length == byte_remainder1)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string.Length; ////o(1)-(put index in array && shift)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        redIndex++; // o(1)-addition && assignment 
                        byte_remainder1 = 8; //o(1)-assignment
                    }
                    else
                    {
                        rf = huffman_string.Substring(0, byte_remainder1);  //o(1) - assignment && substring
                        redBinaryRepresentationToWriteInFile[redIndex] <<= byte_remainder1; //o(1)-(put index in array && shift)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(rf, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        redIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder1, huffman_string.Length - byte_remainder1);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            rf = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            redBinaryRepresentationToWriteInFile[redIndex] <<= 8; //o(1)-(put index in array && shift)
                            redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(rf, 2); //o(1)-(put index in array && assignment && addition && convert)
                            redIndex++; //o(1)-addition && assignment
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); //o(1) - assignment && substring
                        }
                        if (huffman_string.Length != 0) //o(1)
                        {
                            redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                            redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                            byte_remainder1 = 8 - huffman_string.Length; // o(1) - assignment
                        }
                        else
                        {

                            byte_remainder1 = 8; //o(1) - assignment
                        }

                    }

                    // blue channel
                    huffman_string = blue_dict[ImageMatrix[i, j].blue]; // temp = huffman representation of pixel 0
                    if (huffman_string.Length < byte_remainder2) // if length of bits < 8 then arr[0] <<= number of bits (<8)
                    {
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                                                                                                    // 10101, arr[idx] = 0000 0000 => 0000 0000
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                               // 0001 0101
                        byte_remainder2 -= huffman_string.Length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string.Length == byte_remainder2)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string.Length; ////o(1)-(put index in array && shift)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        blueIndex++; // o(1)-addition && assignment 
                        byte_remainder2 = 8; //o(1)-assignment
                    }
                    else
                    {
                        rf = huffman_string.Substring(0, byte_remainder2); //o(1) - assignment && substring
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= byte_remainder2; //o(1)-(put index in array && shift)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(rf, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        blueIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder2, huffman_string.Length - byte_remainder2);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            rf = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            blueBinaryRepresentationToWriteInFile[blueIndex] <<= 8; //o(1)-(put index in array && shift)
                            blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(rf, 2); //o(1)-(put index in array && assignment && addition && convert)
                            blueIndex++; //o(1)-addition && assignment
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); //o(1) - assignment && substring
                        }
                        if (huffman_string.Length != 0) //o(1)
                        {
                            blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                            blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                            byte_remainder2 = 8 - huffman_string.Length; // o(1) - assignment
                        }
                        else
                            byte_remainder2 = 8; //o(1) - assignment
                    }

                    // green channel
                    huffman_string = green_dict[ImageMatrix[i, j].green]; // temp = huffman representation of pixel 0
                    if (huffman_string.Length < byte_remainder3) // if length of bits < 8 then arr[0] <<= number of bits (<8)
                    {
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                                                                                                      // 10101, arr[idx] = 0000 0000 => 0000 0000
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                                 // 0001 0101
                        byte_remainder3 -= huffman_string.Length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string.Length == byte_remainder3)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string.Length; ////o(1)-(put index in array && shift)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        greenIndex++; // o(1)-addition && assignment 
                        byte_remainder3 = 8; //o(1)-assignment
                    }
                    else
                    {
                        rf = huffman_string.Substring(0, byte_remainder3); //o(1) - assignment && substring
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= byte_remainder3; //o(1)-(put index in array && shift)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(rf, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        greenIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder3, huffman_string.Length - byte_remainder3);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            rf = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            greenBinaryRepresentationToWriteInFile[greenIndex] <<= 8; //o(1)-(put index in array && shift)
                            greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(rf, 2); //o(1)-(put index in array && assignment && addition && convert)
                            greenIndex++; //o(1)-addition && assignment
                            huffman_string = huffman_string.Substring(8, huffman_string.Length - 8); //o(1) - assignment && substring
                        }

                        if (huffman_string.Length != 0) //o(1)
                        {
                            greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string.Length; //o(1)-(put index in array && shift)
                            greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                            byte_remainder3 = 8 - huffman_string.Length; // o(1) - assignment
                        }
                        else
                        {
                            byte_remainder3 = 8; //o(1) - assignment
                        }
                    }

                }
            }

            FileStream ss = new FileStream(BinaryPath, FileMode.Truncate);
            BinaryWriter binWriter = new BinaryWriter(ss);

            binWriter.Write(redBinaryRepresentationToWriteInFile);//o(1) (write in file)
            binWriter.Write(blueBinaryRepresentationToWriteInFile);//o(1) (write in file)
            binWriter.Write(greenBinaryRepresentationToWriteInFile);//o(1) (write in file)  

            binWriter.Close();
            ss.Close();

            return total_bytes;
        }

        // function to decompress the image using the huffman tree and the binary file
        public static RGBPixel[,] DecompressImage(string FilePath)
        {
            // declare the binary file stream and the binary reader
            FileStream readingStream = new FileStream(FilePath, FileMode.Open);
            FileStream binary_file_stream = new FileStream(FilePath, FileMode.Open);
            BinaryReader binary_reader = new BinaryReader(binary_file_stream);

            // read the seed value and tap position from the binary file
            seedValue = binary_reader.ReadString();
            seedKey = binary_reader.ReadInt32();

            // read the huffman tree from the binary file
            int red_dict_size = binary_reader.ReadInt32();
            Dictionary<int, string> red_dict = new Dictionary<int, string>();

            for (int i = 0; i < red_dict_size; i++)
            {
                int key = binary_reader.ReadInt32();
                string value = binary_reader.ReadString();
                red_dict.Add(key, value);
            }

            int blue_dict_size = binary_reader.ReadInt32();
            Dictionary<int, string> blue_dict = new Dictionary<int, string>();

            for (int i = 0; i < blue_dict_size; i++)
            {
                int key = binary_reader.ReadInt32();
                string value = binary_reader.ReadString();
                blue_dict.Add(key, value);
            }

            int green_dict_size = binary_reader.ReadInt32();
            Dictionary<int, string> green_dict = new Dictionary<int, string>();

            for (int i = 0; i < green_dict_size; i++)
            {
                int key = binary_reader.ReadInt32();
                string value = binary_reader.ReadString();
                green_dict.Add(key, value);
            }

            // read the total bytes of the image from the binary file
            long total_bytes = binary_reader.ReadInt64();

            // read the compression ratio of the image from the binary file
            double compression_ratio = binary_reader.ReadDouble();

            // read the width and height of the image from the binary file
            int Width = binary_reader.ReadInt32();
            int Height = binary_reader.ReadInt32();

            // declare the image matrix

            RGBPixel[,] ImageMatrix = new RGBPixel[Height, Width];

            // read the image matrix from the binary file

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    // read the red channel from the binary file
                    int red = 0;
                    string red_code = String.Empty;
                    while (true)
                    {
                        red_code += binary_reader.ReadChar();
                        foreach (KeyValuePair<int, string> entry in red_dict)
                        {
                            if (entry.Value == red_code)
                            {
                                red = entry.Key;
                                break;
                            }
                        }
                        if (red != 0)
                        {
                            break;
                        }
                    }

                    // read the green channel from the binary file
                    int green = 0;
                    string green_code = String.Empty;
                    while (true)
                    {
                        green_code += binary_reader.ReadChar();
                        foreach (KeyValuePair<int, string> entry in green_dict)
                        {
                            if (entry.Value == green_code)
                            {
                                green = entry.Key;
                                break;
                            }
                        }
                        if (green != 0)
                        {
                            break;
                        }
                    }

                    // read the blue channel from the binary file
                    int blue = 0;
                    string blue_code = String.Empty;
                    while (true)
                    {
                        blue_code += binary_reader.ReadChar();
                        foreach (KeyValuePair<int, string> entry in blue_dict)
                        {
                            if (entry.Value == blue_code)
                            {
                                blue = entry.Key;
                                break;
                            }
                        }
                        if (blue != 0)
                        {
                            break;
                        }
                    }

                    // update the image matrix with the pixel
                    ImageMatrix[i, j].red = (byte)red;
                    ImageMatrix[i, j].green = (byte)green;
                    ImageMatrix[i, j].blue = (byte)blue;
                }
            }

            // close the binary reader
            binary_reader.Close();

            return ImageMatrix;
        }

    }
}