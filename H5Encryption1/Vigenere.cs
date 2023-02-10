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

        public static string Decrypt(string encryptedMessage, string messageKey)
        {
            return Converter(encryptedMessage, messageKey, false);
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

    public static class VigenereAnalysis
    {
        private static double[] freqNorm = new double[]
            { //letters, A-Z
                0.64297, 0.11746, 0.21902, 0.33483, 1.00000, 0.17541,
                0.15864, 0.47977, 0.54842, 0.01205, 0.06078, 0.31688, 0.18942,
                0.53133, 0.59101, 0.15187, 0.00748, 0.47134, 0.49811, 0.71296,
                0.21713, 0.07700, 0.18580, 0.01181, 0.15541, 0.00583
            };

        public static string FindPossibleKey(string encryptedMessage, uint range)
        { //split into sub functions when done

            var encryptedMessageNoSpace = new string(encryptedMessage.Where(x => x != ' ').ToArray());
            if (string.IsNullOrWhiteSpace(encryptedMessageNoSpace) || encryptedMessageNoSpace.Length == 1 || range >= encryptedMessageNoSpace.Length || range == 0)
                throw new Exception();

            // First calculate the shifted strings and then find the coincidence
            List<string> shiftedStrings = new();
            for(int i = 0; i < encryptedMessageNoSpace.Length -1; i++)
            { // Left shifting
                shiftedStrings.Add(encryptedMessageNoSpace[..(encryptedMessageNoSpace.Length - (i +1))]); //this could be moved down into the for loop below
            }

            List<int> coincidences = new();
            for(int i = 0; i < encryptedMessageNoSpace.Length - 1; i++)
            { // Find the coincidences
                var splitedEncryptedMessage = encryptedMessageNoSpace[(i+1)..];
                int coincidence = 0;
                for(int n = 0; n < splitedEncryptedMessage.Length; n++)
                {
                    if (splitedEncryptedMessage[n] == shiftedStrings[i][n])
                        coincidence++;
                }
                coincidences.Add(coincidence);
            }

            // Find the biggest numbers to get an idea about key length

            var orderedCoincidences = coincidences.OrderByDescending(x => x).ToArray();
            //how to get an idea about what is a 'big' value
            //double cutoffValue = orderedCoincidences.First() - orderedCoincidences.First() * 0.1;
            //var keyLength = orderedCoincidences.Where(x => x >= cutoffValue).Count();

            var possibleKeyRanges = new int[range];
            for(int i = 0; i < range; i++)
            {
                possibleKeyRanges[i] = i + 1;
            }

            // Find all letters at each key length and collect them 
            //if key e.g. is 4, look at 0, 4, 8, 12 etc and count each letter. Then shift to the next location and count again, 1, 5, 9, 13 etc, then again 2, 6, 10, 14 and finally 3, 7, 11 and 15
            //if key e.g. is 2, look at 0, 2, 4, 6 and then shift with one, 1, 3, 5, 7
            foreach(var keyRange in possibleKeyRanges)
            { //currently working at the around 4:20 mins left mark of the video
                var letters = encryptedMessageNoSpace.Select((value, index) => new { index, value }).Where(x => x.index % keyRange == 0).Select(x => x.value);
                var letterCount = letters.Count();
                var letterAmount = letters.GroupBy(x => x).Select(x => { var count = x.Count(); return new { x.Key, count }; });
                //when calculating the biggest number, need to deal with the cases where not all letters are present, so get a small freqNorm array without those missing letters


            }

            throw new NotImplementedException();
        }
    }
}
