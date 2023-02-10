using System.Text;
using Test;

namespace H5Encryption1
{
    public static class Vigenere
    {
        public static string Encrypt(string message, string messageKey)
        {
            return Converter(message, messageKey, true);
        }

        public static string Decrypt(string message, string messageKey)
        {
            return Converter(message, messageKey, false);
        }


        private static string Converter(string message, string keyMessage, bool encrypt)
        {
            StringBuilder sb = new();
            int currentKeyIndex = 0;
            int keyIndexLength = keyMessage.Length;
            string formattedMessage = Caesar.FormatMessage(message);
            keyMessage = keyMessage.ToUpper();
            foreach (var chr in formattedMessage)
            {
                if (chr == ' ')
                {
                    sb.Append(chr);
                    continue;
                }
                byte key = (byte)(keyMessage[currentKeyIndex] - 65);
                if (key > 25)
                {
                    key = (byte)(key - 26);
                }
                string converted = encrypt ? Caesar.Encrypt(chr.ToString(), key) : Caesar.Decrypt(chr.ToString(), key);
                sb.Append(converted);

                currentKeyIndex = ++currentKeyIndex == keyIndexLength ? 0 : currentKeyIndex;
            }


            return sb.ToString();
        }
    }
}
