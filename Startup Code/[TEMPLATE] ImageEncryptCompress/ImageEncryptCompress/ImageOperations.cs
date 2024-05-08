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
        public static bool isEncrypted = false;

        // path for text file of the huffman tree and some other data
        public static string CompressionPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\RGB-Tree.txt";

        // path for binary file
        public static string BinaryWriterPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\Binary.bin";

        // paths for the encrypted and decrypted images
        public static string compressedImageDataPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\com.txt";

        // paths for the encrypted and decrypted images
        public static string EncryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Encryption\\Encrypted.bmp";
        public static string DecryptedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Encryption\\MY_OUTPUT\\Decryption\\Decrypted.bmp";

        // paths for the compressed and decompressed images
        public static string DecompressedImagePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Decompression\\Decompressed.bmp";

        // paths for the huffman representations for each color channel of each pixel
        public static string CompressedRedPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\R-Tree.txt";
        public static string CompressedGreenPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\G-Tree.txt";
        public static string CompressedBluePath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\B-Tree.txt";

        // paths for compressed binary strings for each color channel of each pixel
        public static string CompressedRedBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\R-Binary.bin";
        public static string CompressedGreenBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\G-Binary.bin";
        public static string CompressedBlueBinaryPath = "D:\\Study\\Third Year\\Semester 6\\Algo\\Project\\Image_Encryption_Compression\\Sample Test\\SampleCases_Compression\\MY_OUTPUT\\Compression\\B-Binary.bin";

        public static int red_length = 0, green_length = 0, blue_length = 0, Tape_Position = 0;
        public static int[] R, G, B;
        public static float matrix_dimintion = 0;
        public static long red_bytes = 0, green_bytes = 0, blue_bytes = 0, Total_Bits = 0;
        public static string Initial_Seed = "";
        public static string[] arr_red, arr_gr, arr_bl;

        public static byte[] arr, arr1, arr2;

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

            // create string builder to store the answer
            StringBuilder answer = new StringBuilder();
            StringBuilder seedBuilder = new StringBuilder(initialSeed);

            // convert the initial seed to binary
            int size = initialSeed.Length;
            char tapPositionBit;
            char firstBit;

            int cnt = 0;
            for (int i = 1; i <= k * 3; i++)
            {

                tapPositionBit = seedBuilder[size - tapPosition - 1];
                firstBit = seedBuilder[0];
                char newBit = helpers.XOR(firstBit, tapPositionBit);

                answer.Append(newBit);

                if (i % k == 0)
                {
                    results[cnt] = answer.ToString();
                    answer.Clear();
                    cnt++;
                }

                // shifting the bits to the right
                seedBuilder.Remove(0, 1);
                seedBuilder.Append(newBit);

                // printing the new seed
                //Console.Write(i + 1 + " ==> ");
                //Console.WriteLine(initialSeed);
            }

            seedValue = seedBuilder.ToString();

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
            string[] keys;

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
                    byte redBinaryByte = Convert.ToByte(keys[0], 2);
                    byte greenBinaryByte = Convert.ToByte(keys[1], 2);
                    byte blueBinaryByte = Convert.ToByte(keys[2], 2);

                    byte encryptedRedByte = (byte)(red ^ redBinaryByte);
                    byte encryptedGreenByte = (byte)(green ^ greenBinaryByte);
                    byte encryptedBlueByte = (byte)(blue ^ blueBinaryByte);
                    

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


            if (root.Left == null && root.Right == null)
            {
                dict.Add(root.Pixel, s);

                int bittat = s.Length * root.Frequency;

                stream.WriteLine(root.Pixel + " " + s);

                Total_Bits += bittat;

            }
        }

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
            long red_bytes = red_total_bits / 8;
            long red_rem = (red_total_bits % 8);
            if (red_rem != 0)
                red_bytes++;


            // green channel
            // green_total_bits += (green_total_bits % 8);
            long green_bytes = green_total_bits / 8;
            long green_rem = (green_total_bits % 8);
            if (green_rem != 0)
                green_bytes++;


            // blue channel
            long blue_bytes = blue_total_bits / 8;
            long blue_rem = (blue_total_bits % 8);
            if (blue_rem != 0)
                blue_bytes++;

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


            // law fy remainder ehgez bytes + 1

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

            // if ( redindex == redbytes && i == height-1 && j == width-1 )

            // variables to store the huffman representation of the pixel
            string huffman_string, huffman_substr;
            int huffman_string_length;
            for (int i = 0; i < Height; i++) //o(h*w)
            {
                for (int j = 0; j < Width; j++) //o(w)
                {
                    // red channel

                    // get the huffman representation of the pixel
                    huffman_string = red_dict[ImageMatrix[i, j].red]; // temp = huffman representation of pixel 0
                    huffman_string_length = huffman_string.Length;
                    // if the length of the huffman representation is less than the remaining bits in the byte
                    if (huffman_string_length < byte_remainder1)
                    {
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string_length; //o(1)-(put index in array && shift)
                                                                                                  // 10101, arr[idx] = 0000 0000 => 0000 0000
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                             // 0001 0101
                        byte_remainder1 -= huffman_string_length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string_length == byte_remainder1)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        redBinaryRepresentationToWriteInFile[redIndex] <<= huffman_string_length; ////o(1)-(put index in array && shift)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        redIndex++; // o(1)-addition && assignment 
                        byte_remainder1 = 8; //o(1)-assignment
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder1);  //o(1) - assignment && substring
                        redBinaryRepresentationToWriteInFile[redIndex] <<= byte_remainder1; //o(1)-(put index in array && shift)
                        redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_substr, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        redIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder1, huffman_string.Length - byte_remainder1);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            huffman_substr = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            redBinaryRepresentationToWriteInFile[redIndex] <<= 8; //o(1)-(put index in array && shift)
                            redBinaryRepresentationToWriteInFile[redIndex] += Convert.ToByte(huffman_substr, 2); //o(1)-(put index in array && assignment && addition && convert)
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
                    huffman_string_length = huffman_string.Length;
                    if (huffman_string_length < byte_remainder2) // if length of bits < 8 then arr[0] <<= number of bits (<8)
                    {
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string_length; //o(1)-(put index in array && shift)
                                                                                                    // 10101, arr[idx] = 0000 0000 => 0000 0000
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                               // 0001 0101
                        byte_remainder2 -= huffman_string_length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string_length == byte_remainder2)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= huffman_string_length; ////o(1)-(put index in array && shift)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        blueIndex++; // o(1)-addition && assignment 
                        byte_remainder2 = 8; //o(1)-assignment
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder2); //o(1) - assignment && substring
                        blueBinaryRepresentationToWriteInFile[blueIndex] <<= byte_remainder2; //o(1)-(put index in array && shift)
                        blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_substr, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        blueIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder2, huffman_string_length - byte_remainder2);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            huffman_substr = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            blueBinaryRepresentationToWriteInFile[blueIndex] <<= 8; //o(1)-(put index in array && shift)
                            blueBinaryRepresentationToWriteInFile[blueIndex] += Convert.ToByte(huffman_substr, 2); //o(1)-(put index in array && assignment && addition && convert)
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
                    huffman_string_length = huffman_string.Length;
                    if (huffman_string_length < byte_remainder3) // if length of bits < 8 then arr[0] <<= number of bits (<8)
                    {
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string_length; //o(1)-(put index in array && shift)
                                                                                                      // 10101, arr[idx] = 0000 0000 => 0000 0000
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                                                                                                                 // 0001 0101
                        byte_remainder3 -= huffman_string_length; //o(1)-subtraction && assignment
                    }
                    else if (huffman_string_length == byte_remainder3)
                    {
                        // temp = "101"
                        // ar = 8 - 5 = 3
                        // hn shift left 00010101 f teb2a 10101000
                        // "101" = 3 => arr[idx] = 10101000 => arr[idx] += "101" => 1010 1101
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= huffman_string_length; ////o(1)-(put index in array && shift)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_string, 2); //o(1)-(put index in array && assignment && addition && convert)
                        greenIndex++; // o(1)-addition && assignment 
                        byte_remainder3 = 8; //o(1)-assignment
                    }
                    else
                    {
                        huffman_substr = huffman_string.Substring(0, byte_remainder3); //o(1) - assignment && substring
                        greenBinaryRepresentationToWriteInFile[greenIndex] <<= byte_remainder3; //o(1)-(put index in array && shift)
                        greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_substr, 2);  //o(1)-(put index in array && assignment && addition && convert)
                        greenIndex++; //o(1)-addition && assignment
                        huffman_string = huffman_string.Substring(byte_remainder3, huffman_string_length - byte_remainder3);//o(1) - assignment && substring

                        while (huffman_string.Length >= 8) //o(1) AS temp size is limited to 32
                        {
                            huffman_substr = huffman_string.Substring(0, 8); //o(1)-assignment && substring
                            greenBinaryRepresentationToWriteInFile[greenIndex] <<= 8; //o(1)-(put index in array && shift)
                            greenBinaryRepresentationToWriteInFile[greenIndex] += Convert.ToByte(huffman_substr, 2); //o(1)-(put index in array && assignment && addition && convert)
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

            byte[] redFreqByteArr = new byte[1024];//o(1) (assignment)
            byte[] greenFreqByteArr = new byte[1024];//o(1) (assignment)
            byte[] blueFreqByteArr = new byte[1024];//o(1) (assignment)

            for (int i = 0; i < 256; i++)
            {
                Array.Copy(BitConverter.GetBytes(redFreq[i]), 0, redFreqByteArr, i * 4, 4);//o(nlon)(number of iterations*4 && copy to array)
                Array.Copy(BitConverter.GetBytes(greenFreq[i]), 0, greenFreqByteArr, i * 4, 4);//o(nlon)(number of iterations*4 && copy to array)
                Array.Copy(BitConverter.GetBytes(blueFreq[i]), 0, blueFreqByteArr, i * 4, 4);//o(nlon)(number of iterations*4 && copy to array)
            }

            FileStream ffs = new FileStream(compressedImageDataPath, FileMode.Truncate);
            StreamWriter ffss = new StreamWriter(ffs);
            ffss.WriteLine(red_bytes);//o(1) (write in file)
            ffss.WriteLine(green_bytes);//o(1) (write in file)
            ffss.WriteLine(blue_bytes);//o(1) (write in file)

            ffss.WriteLine(red_rem);//o(1) (write in file)
            ffss.WriteLine(green_rem);//o(1) (write in file)
            ffss.WriteLine(blue_rem);//o(1) (write in file)

            ffss.Close();
            ffs.Close();

            FileStream ss = new FileStream(BinaryWriterPath, FileMode.Truncate);
            BinaryWriter binWriter = new BinaryWriter(ss);


            binWriter.Write(redFreqByteArr);
            binWriter.Write(greenFreqByteArr);
            binWriter.Write(blueFreqByteArr);

            binWriter.Write(redBinaryRepresentationToWriteInFile);//o(1) (write in file)
            binWriter.Write(greenBinaryRepresentationToWriteInFile);//o(1) (write in file)  
            binWriter.Write(blueBinaryRepresentationToWriteInFile);//o(1) (write in file)

            binWriter.Write(seedValue);
            binWriter.Write(tapPosition);

            binWriter.Write(Width);
            binWriter.Write(Height);

            binWriter.Close();
            ss.Close();

            return total_bytes;
        }

        // function to decompress the image using the huffman tree and the binary file
        public static RGBPixel[,] DecompressImage()
        {
            // declare the binary file stream and the binary reader
            FileStream readingStream = new FileStream(compressedImageDataPath, FileMode.Open);
            StreamReader stream_reader = new StreamReader(readingStream);

            // stream carries:
            // rgb length (3 lines)
            // binary carries:
            // (1) rgb frequencies (each one 1024 byte)
            // (2) huffman representations (3) seed (4) tap position (5) width (6) height

            // lengths of rgb bytes
            int red_length = Convert.ToInt32(stream_reader.ReadLine());
            int green_length = Convert.ToInt32(stream_reader.ReadLine());
            int blue_length = Convert.ToInt32(stream_reader.ReadLine());

            int red_extra_bits = Convert.ToInt32(stream_reader.ReadLine());
            int green_extra_bits = Convert.ToInt32(stream_reader.ReadLine());
            int blue_extra_bits = Convert.ToInt32(stream_reader.ReadLine());

            stream_reader.Close();
            readingStream.Close();

            FileStream binaryReadingStream = new FileStream(BinaryWriterPath, FileMode.Open);
            BinaryReader binary_reader = new BinaryReader(binaryReadingStream);

            // frequency arrs for carrying freqs that are the 
            byte[] redFreqInBytes = binary_reader.ReadBytes(1024);
            byte[] greenFreqInBytes = binary_reader.ReadBytes(1024);
            byte[] blueFreqInBytes = binary_reader.ReadBytes(1024);

            // rgb arrs to store 
            int[] redFreq = new int[256];
            int[] greenFreq = new int[256];
            int[] blueFreq = new int[256];

            PriorityQueue pq_red = new PriorityQueue();
            PriorityQueue pq_green = new PriorityQueue();
            PriorityQueue pq_blue = new PriorityQueue();

            for (int i = 0; i < 1024; i += 4)//o(1) this loop take 4 bytes and compress them to one int32 value
            {
                redFreq[i / 4] = BitConverter.ToInt32(redFreqInBytes, i);
                greenFreq[i / 4] = BitConverter.ToInt32(greenFreqInBytes, i);
                blueFreq[i / 4] = BitConverter.ToInt32(blueFreqInBytes, i);
            }

            for (int i = 0; i < 256; i++)//o(1) this loop add in the lists that it's value is not 0
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

            // construct the huffman tree for the red channel
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

            HuffmanNode rootNodeRed = pq_red.Pop();
            HuffmanNode rootNodeGreen = pq_green.Pop();
            HuffmanNode rootNodeBlue = pq_blue.Pop();

            //o(1) read from the file the red bytes compressed values
            byte[] compressed_red = binary_reader.ReadBytes(red_length);


            //o(1) read from the file the green bytes compressed values
            byte[] compressed_green = binary_reader.ReadBytes(green_length);


            //o(1) read from the file the blue bytes compressed values
            byte[] compressed_blue = binary_reader.ReadBytes(blue_length);

            seedValue = binary_reader.ReadString();// o(1) get the seed from the file 
            seedKey = binary_reader.ReadInt32();// o(1) get the tape from the file

            int Width = binary_reader.ReadInt32();// o(1) get the width from the file
            int Height = binary_reader.ReadInt32();// o(1) get the heigth from the file 

            binary_reader.Close();
            binaryReadingStream.Close();
            //            Tape_Position = tap_position;//o(1) save it to he global value
            //            Initial_Seed = seed;//o(1) save it to the global value            

            // 3 lists to save colors values that would end in 3*(N^2) space
            List<int> redPixels = new List<int>();//o(1) 
            List<int> greenPixels = new List<int>();//o(1) 
            List<int> bluePixels = new List<int>();//o(1) 

            byte byteValue = 128; //o(1) assigment to get the bits from the byte color
            //1st iter: 1000 0000, 2nd: 0100 0000, 3rd: 0010 0000, 4th: 0001 0000, 5th: 0000 1000, 6th: 0000 0100
            int currBitCount = 0; // o(1) assigment that count to 8 to get the value of one byte
            HuffmanNode rootNode1 = rootNodeRed; // o(1) assigment that point to the top node in huffman

            int cnt = 0;
            long crl = compressed_red.Length;

            while (cnt < crl) //o(s.length)
            {
                while (currBitCount < 8) //o(1) this loop count until the byte read all 
                {
                    byte hettaOS = (byte)(compressed_red[cnt] & byteValue); //o(1) assigmrnt to get the spceific bit
                    HuffmanNode tempNode; //o(1) call the function to get the next node according to the bit value
                    if (hettaOS == 0) //o(1) Assigment
                        tempNode = rootNode1.Left; //o(1)return
                    else
                        tempNode = rootNode1.Right; //o(1)return

                    if (tempNode.Left == null && tempNode.Right == null) //o(1) check that the cuurent node is a leaf node
                    {
                        redPixels.Add(tempNode.Pixel); // o(1) add to the list the value of the cuurent color
                        rootNode1 = rootNodeRed; // O(1) make search start from the root or the huffman
                    }
                    else
                    {
                        rootNode1 = tempNode; //o(1)make the start node is the current node
                    }

                    byteValue /= 2; // o(1)  divide the var to get the next bit 
                    currBitCount++; // o(1) read another 8 bits
                }

                cnt++; // o(1) go to the next list value to save on it
                currBitCount = 0; // o(1) reset the counter
                byteValue = 128; // o(1) reset the var to make it point to the first bit
            }

            byteValue = 128;
            currBitCount = 0;
            rootNode1 = rootNodeGreen;
            cnt = 0;
            long cgl = compressed_green.Length;

            while (cnt < cgl) //o(s.length)
            {
                while (currBitCount < 8) //o(1) this loop count until the byte read all 
                {
                    byte hettaOS = (byte)(compressed_green[cnt] & byteValue); //o(1) assigmrnt to get the spceific bit
                    HuffmanNode tempNode; //o(1) call the function to get the next node according to the bit value
                    if (hettaOS == 0) //o(1) Assigment
                        tempNode = rootNode1.Left; //o(1)return
                    else
                        tempNode = rootNode1.Right; //o(1)return

                    if (tempNode.Left == null && tempNode.Right == null) //o(1) check that the cuurent node is a leaf node
                    {
                        greenPixels.Add(tempNode.Pixel); // o(1) add to the list the value of the cuurent color
                        rootNode1 = rootNodeGreen; // O(1) make search start from the root or the huffman
                    }
                    else
                    {
                        rootNode1 = tempNode; //o(1)make the start node is the current node
                    }

                    byteValue /= 2; // o(1)  divide the var to get the next bit 
                    currBitCount++; // o(1) read another 8 bits

                }
                cnt++; // o(1) go to the next list value to save on it
                currBitCount = 0; // o(1) reset the counter
                byteValue = 128; // o(1) reset the var to make it point to the first bit
            }

            byteValue = 128;
            currBitCount = 0;
            rootNode1 = rootNodeBlue;
            cnt = 0;
            long cbl = compressed_blue.Length;

            while (cnt < cbl) //o(s.length)
            {
                while (currBitCount < 8) //o(1) this loop count until the byte read all 
                {
                    byte hettaOS = (byte)(compressed_blue[cnt] & byteValue); //o(1) assigmrnt to get the spceific bit
                    HuffmanNode tempNode; //o(1) call the function to get the next node according to the bit value
                    if (hettaOS == 0) //o(1) Assigment
                        tempNode = rootNode1.Left; //o(1)return
                    else
                        tempNode = rootNode1.Right; //o(1)return

                    if (tempNode.Left == null && tempNode.Right == null) //o(1) check that the cuurent node is a leaf node
                    {
                        bluePixels.Add(tempNode.Pixel); // o(1) add to the list the value of the cuurent color
                        rootNode1 = rootNodeBlue; // O(1) make search start from the root or the huffman
                    }
                    else
                    {
                        rootNode1 = tempNode; //o(1)make the start node is the current node
                    }

                    byteValue /= 2; // o(1)  divide the var to get the next bit 
                    currBitCount++; // o(1) read another 8 bits
                }
                cnt++; // o(1) go to the next list value to save on it
                currBitCount = 0; // o(1) reset the counter
                byteValue = 128; // o(1) reset the var to make it point to the first bit
            }

            RGBPixel[,] decompressedPicture = new RGBPixel[Height, Width];
            int index = 0;

            int redLength = redPixels.Count;
            int greenLength = greenPixels.Count;
            int blueLength = bluePixels.Count;

            for (int i = 0; i < Height; i++)//o(H)
            {
                for (int j = 0; j < Width; j++)//o(W)
                {
                    if (index < redLength && index < greenLength && index < blueLength)
                    {
                        decompressedPicture[i, j].red = (byte)redPixels[index];
                        decompressedPicture[i, j].green = (byte)greenPixels[index];
                        decompressedPicture[i, j].blue = (byte)bluePixels[index];
                    }
                    index++;
                }
            }

            return decompressedPicture;
        }
    }
}