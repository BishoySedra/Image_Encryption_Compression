using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{
    internal class BONUS_Functions
    {
        // [BONUS] function to convert the string to binary
        // O(n) where n is the length of the string
        public static string AlphanumericConvertion(string data)
        {
            string binary = string.Empty;
            int data_size = data.Length;

            // if it has 0's or 1's in data string, store it in binary
            foreach (char c in data)
            {
                if (c == '0' || c == '1')
                {
                    binary += c;
                }
            }

            int binary_size = binary.Length;

            if (binary_size == data_size)
            {
                return binary;
            }

            string converted_result = String.Empty;
            for (int i = 0; i < data_size; i++)
            {
                // convert the character to binary
                string binary_char = Convert.ToString(data[i], 2);
                Console.WriteLine("Character: " + data[i] + " Binary: " + binary_char);
                converted_result += binary_char;
            }

            return converted_result;
        }

        public static string[] GetCombinations(int length) // θ(2^length)
        {

            // get the total number of combinations
            int total = (int)Math.Pow(2, length);

            // create an array to store the combinations
            string[] combinations = new string[total];

            // loop through the total number of combinations
            for (int i = 0; i < total; i++)
            {
                // convert the number to binary
                string binary = Convert.ToString(i, 2);

                // get the length of the binary string
                int binary_length = binary.Length;

                // check if the length of the binary string is less than the required length
                if (binary_length < length)
                {
                    // add zeros to the left of the binary string to make it the required length
                    binary = binary.PadLeft(length, '0');
                }

                // add the binary string to the combinations array
                combinations[i] = binary;
            }

            // return the combinations array
            return combinations;
        }

        // [BONUS] function to attack the encrypted image
        public static Tuple<string, int> Attack(RGBPixel[,] EncryptedImageMatrix, RGBPixel[,] DesiredImageMatrix, int Nbits) // O(2^n * n * m)
        {
            // get all the possible combinations of the initial seed
            string[] combinations = GetCombinations(Nbits);

            // loop through all the combinations
            foreach (string combination in combinations)
            {
                // loop through all the tap positions
                for (int tapPosition = 0; tapPosition < Nbits; tapPosition++)
                {
                    // decrypt the image using the current combination and tap position
                    RGBPixel[,] DecryptedImageMatrix = ImageOperations.EncryptDecryptImage(EncryptedImageMatrix, combination, tapPosition);

                    // check if the decrypted image is identical to the desired image
                    if (TestIdenticality(DecryptedImageMatrix, DesiredImageMatrix))
                    {
                        // return the combination and tap position
                        return new Tuple<string, int>(combination, tapPosition);
                    }
                }
            }

            // return null if no combination and tap position were found
            return null;

        }

        // function to test the identicality of two images
        // O(n * m) where n is the height of the image and m is the width of the image
        public static bool TestIdenticality(RGBPixel[,] ImageMatrix1, RGBPixel[,] ImageMatrix2)
        {
            int Height1 = ImageMatrix1.GetLength(0);
            int Width1 = ImageMatrix1.GetLength(1);

            int Height2 = ImageMatrix2.GetLength(0);
            int Width2 = ImageMatrix2.GetLength(1);

            // checking if the dimensions of the two images are not the same
            if (Height1 != Height2 || Width1 != Width2)
            {
                return false;
            }


            //if (ImageMatrix1 != ImageMatrix2) { 
            //    return false;
            //}

            // checking the identicality of the two images for each pixel
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
