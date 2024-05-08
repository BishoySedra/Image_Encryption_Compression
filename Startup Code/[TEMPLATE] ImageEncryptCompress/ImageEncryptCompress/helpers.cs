using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{
    internal class helpers
    {
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

        // function to do XORing between two characters 
        public static char XOR(char bit_1, char bit_2) // O(1)
        {
            return (bit_1 == bit_2) ? '0' : '1';
        }

        // function to do Xoring between two bytes
        public static byte XOR(byte a, byte b) // O(1)
        {
            return (byte)(a ^ b);
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

        // function to convert from binary string to byte
        public static byte convertToByte(string binary) // O(1)
        {
            return Convert.ToByte(binary, 2);
        }


    }
}
