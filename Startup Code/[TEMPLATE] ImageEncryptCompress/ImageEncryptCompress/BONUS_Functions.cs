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
            //string binary = string.Empty;
            StringBuilder binary = new StringBuilder(); // O(1)
            int data_size = data.Length; // O(1)

            // if it has 0's or 1's in data string, store it in binary
            foreach (char c in data) // O(n) where n is the length of the string
            {
                if (c == '0' || c == '1')
                {
                    binary.Append(c); // O(1)
                }
            }

            int binary_size = binary.Length; // O(1)

            if (binary_size == data_size)
            {
                return binary.ToString(); // O(1)
            }

            //string converted_result = String.Empty;
            StringBuilder converted_result = new StringBuilder(); // O(1)
            for (int i = 0; i < data_size; i++) // O(n) where n is the length of the string
            {
                // convert the character to binary
                string binary_char = Convert.ToString(data[i], 2); // O(log(n))
                //Console.WriteLine("Character: " + data[i] + " Binary: " + binary_char);
                //converted_result += binary_char;
                converted_result.Append(binary_char); // O(1)
            }

            return converted_result.ToString(); // O(1)
        }

        // function to get all the possible combinations of a given length
        // O(2^n * n) where n is the length of the binary string
        public static string[] GetCombinations(int length)
        {

            // total number of combinations
            int total = (int)Math.Pow(2, length); // O(1) or O(log(n))

            // array to store the combinations
            string[] combinations = new string[total]; // O(1)

            // iterate over all the possible combinations
            // O(2^n * n) where n is the length of the binary string
            for (int i = 0; i < total; i++)
            {
                // convert the number to binary
                string binary = Convert.ToString(i, 2); // O(log(n))

                // get the length of the binary string
                int binary_length = binary.Length; // O(1)

                // check if the length of the binary string is less than the required length
                if (binary_length < length) // O(1)
                {
                    // add zeros to the left of the binary string to make it the required length
                    binary = binary.PadLeft(length, '0'); // O(n)
                }

                // add the binary string to the combinations array
                combinations[i] = binary; // O(1)
            }

            // return the combinations array
            return combinations; // O(1)
        }

        // [BONUS] function to attack the encrypted image
        // O(2^n * n * m) where n is the length of the binary string and m is the size of the image
        public static Tuple<string, int> Attack(RGBPixel[,] EncryptedImageMatrix, RGBPixel[,] DesiredImageMatrix, int Nbits) // O(2^n * n * m)
        {
            // get all the possible combinations of the initial seed
            string[] combinations = GetCombinations(Nbits); // O(2^n * n)

            // iterate over all the possible combinations
            // O(2^n * n * m) where n is the length of the binary string and m is the size of the image
            foreach (string combination in combinations) // O(2^n)
            {
                // loop through all the tap positions
                for (int tapPosition = 0; tapPosition < Nbits; tapPosition++) // O(n)
                {
                    // decrypt the image using the current combination and tap position
                    RGBPixel[,] DecryptedImageMatrix = ImageOperations.EncryptDecryptImage(EncryptedImageMatrix, combination, tapPosition); // O(n * m)

                    // check if the decrypted image is identical to the desired image
                    if (TestIdenticality(DecryptedImageMatrix, DesiredImageMatrix)) // O(n * m)
                    {
                        // return the combination and tap position
                        return new Tuple<string, int>(combination, tapPosition); // O(1)
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
            // get the dimensions of the two images
            int Height1 = ImageOperations.GetHeight(ImageMatrix1); // O(1)
            int Width1 = ImageOperations.GetWidth(ImageMatrix1); // O(1)

            int Height2 = ImageOperations.GetHeight(ImageMatrix2); // O(1)
            int Width2 = ImageOperations.GetWidth(ImageMatrix2); // O(1)
            

            // checking if the dimensions of the two images are not the same
            if (Height1 != Height2 || Width1 != Width2) // O(1)
            {
                return false;
            }

            // checking the identicality of the two images for each pixel
            // O(n * m) where n is the height of the image and m is the width of the image
            for (int i = 0; i < Height1; i++) // O(n)
            {
                for (int j = 0; j < Width1; j++)  // O(m)
                {
                    if (ImageMatrix1[i, j].red != ImageMatrix2[i, j].red || ImageMatrix1[i, j].green != ImageMatrix2[i, j].green || ImageMatrix1[i, j].blue != ImageMatrix2[i, j].blue) // O(1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
