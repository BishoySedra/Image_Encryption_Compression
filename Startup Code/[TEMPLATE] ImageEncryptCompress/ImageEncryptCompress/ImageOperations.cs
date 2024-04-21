using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
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
        public static string[] keys = new string[3];

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
        public static RGBPixel[,] Encrypt(RGBPixel[,] imageMatrix, string InitialSeed, int tapPosition)
        {
            // Generate LFSR keys for encryption
            seedValue = InitialSeed;

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
    }
}
