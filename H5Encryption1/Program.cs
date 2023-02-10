using H5Encryption1;
using System.Text.RegularExpressions;
using Test;

var sf1 = Caesar.FormatMessage(File.ReadAllText("song 1.txt"));
//var se1 = Caesar.Encrypt(sf1, 4);
//Console.WriteLine(se1);
//Console.WriteLine(Caesar.Decrypt(se1, 4));

Console.WriteLine();

var sf2 = Caesar.FormatMessage(File.ReadAllText("song 2.txt"));
//var se2 = Caesar.Encrypt(sf2, 15);
//Console.WriteLine(se2);
//Console.WriteLine(Caesar.Decrypt(se2,15));


//var brutes = CaesarAnalysis.BruteForce(se1);
//foreach(var t in brutes.Keys)
//{
//    Console.WriteLine($"{t}: " + brutes[t]);
//    Console.WriteLine();
//}

//var plain = sf1;
//var text = Caesar.Encrypt(plain,1);
//var key = CaesarAnalysis.FindPossibleKey(text);
//Console.WriteLine($"{key}: " + Caesar.Decrypt(text, key));

//Console.WriteLine();
//var secondBestKey = CaesarAnalysis.FindPossibleKeys(text).ToArray()[1];
//Console.WriteLine($"{secondBestKey}: " + Caesar.Decrypt(text, secondBestKey));


//Console.WriteLine(Vigenere.Encrypt("AAAA", "AB"));
//Console.WriteLine(Vigenere.Encrypt("This is a test", "A"));
//var encrypted = Vigenere.Encrypt("ABCDEFGHIJKLMN", "T");
//Console.WriteLine(encrypted);
//var decrypted = Vigenere.Decrypt(encrypted, "T");
//Console.WriteLine(decrypted);

//Tester("ABCDEFGHIJKLMNZ", "A");
//Tester("ABCDEFGHIJKLMNZ", "B");
//Tester("ABCDEFGHIJKLMNZ", "C");
//Tester("ABCDEFGHIJKLMNZ", "ABC");
//Tester("ABCDEFGHIJKLMNZ", "T");
//static void Tester(string message, string key)
//{
//    Console.WriteLine($"Message: {message}");
//    Console.WriteLine($"Key: {key}");
//    var encrypted = Vigenere.Encrypt(message, key);
//    Console.WriteLine($"Encrypted: {encrypted}");
//    var decrypted = Vigenere.Decrypt(encrypted, key);
//    Console.WriteLine($"Decrypted: {decrypted}{Environment.NewLine}");
//}

VigenereAnalysis.FindPossibleKey(Vigenere.Encrypt(Caesar.FormatMessage(sf1), "ABCD"),10);
//VigenereAnalysis.FindPossibleKey("AABBAABBCCDDTHISISATESTTESTINGSOMETHINGHERETESTESTEST");

namespace Test
{
    public static class CaesarAnalysis
    {
        public static Dictionary<byte, string> BruteForce(string encrypted)
        {
            var dic = new Dictionary<byte, string>();
            for(byte i = 25; i > 0; i--)
            {
                dic.Add(i, Caesar.Decrypt(encrypted, i));
            }
            return dic;
        }

        public static byte FindPossibleKey(string toDecrypt)
        {
            return FindPossibleKeys(toDecrypt).First();
        }

        public static List<byte> FindPossibleKeys(string toDecrypt)
        {
            var freqNorm = new double[]
            { //letters, A-Z
                0.64297, 0.11746, 0.21902, 0.33483, 1.00000, 0.17541,
                0.15864, 0.47977, 0.54842, 0.01205, 0.06078, 0.31688, 0.18942,
                0.53133, 0.59101, 0.15187, 0.00748, 0.47134, 0.49811, 0.71296,
                0.21713, 0.07700, 0.18580, 0.01181, 0.15541, 0.00583
            };
            var noSpace = toDecrypt.Where(c => c != ' ');
            var groupping = noSpace.GroupBy(x => x);
            
            
            //rewrite this function so it return all keys in the order of most likely to less likely, so will need to get the index of each freqnorm
            var freqNormList = freqNorm.ToList();
            //calculate the index of each value 
            var listOfCalculatedFreq = new List<double>();
            var possibleKeys = new List<byte>();
            foreach(var freq in freqNormList)
            {
                var remainedFreq = freqNorm.Where(x => !listOfCalculatedFreq.Any(xx => x == xx));
                var max = remainedFreq.Max();
                listOfCalculatedFreq.Add(max);
                var index = freqNormList.IndexOf(max); //the below code gets the difference between each key (subtracting the ansii value to convert it to a decryption key) and the index of the max freq norm.
                //should be rewritten to be more clean and most likely converted to a loop for improving some checks 
                //The most common letter is the index of max, however, x.key is given in ancii and the index value is just the letter number (starting from 0) so to get the amount of times the letters have been shifted. 
                //currently it can overflow and the fix to the underflow causes problems with 0 and 26 (no shift and shiften back to the plaintext), and should be fixed in proper production code, but it does work for all permitted keys (1-25)
                //do note this is only for the most likely key (so the index freqNorm == 1). It is for the rest the code need some fixing.
                //consider subtracting the 'A' from the x.Key first, since that part of the math makes more sense together, converting first and then deal with the index 
                var keyAmount = groupping.Select(x => { return new {Key = (byte)(x.Key - ('A' + index)) < 26 ? (byte)(x.Key - ('A' + index)) : (byte)(x.Key - ('A' + index))-(255-25), Amount = x.Count() }; }).ToArray();
                possibleKeys.Add((byte)keyAmount.OrderByDescending(x => x.Amount).First().Key); //the key that is used the most is chosen. 
            } //improve the groupping selects as they can underflow, e.g. A - (A+4)

            return possibleKeys;
        }
    }


    public static class Caesar
    {
        public static string FormatMessage(string message)
        {
            Regex regex = new("[\\w ]");
            var formatted = "";
            foreach (var c in message.ToUpper())
            {
                if (regex.IsMatch(c.ToString()))
                {
                    if (!(formatted.Any() && formatted.Last() == ' ' && c == ' '))
                    {
                        formatted += (char)c;
                    }
                }
            }
            return formatted;
        }

        public static string Encrypt(string message, byte key)
        {
            if (key < 0 || key >= 26)
                throw new Exception();
            return Coder(message, key);
        }

        public static string Decrypt(string message, byte key)
        {
            if (key < 0 || key >= 26)
                throw new Exception();
            return Coder(message, -key);
        }

        private static string Coder(string message, int key)
        {
            string encrypted = "";
            foreach (var c in message)
            {
                if (c != ' ')
                {
                    var ce = (char)(c + key);
                    if (ce > 90)
                    {
                        ce = (char)(ce - 26);
                    }
                    else if (ce < 65)
                    {
                        ce = (char)(ce + 26);
                    }
                    encrypted += ce;
                }
                else
                {
                    encrypted += c;
                }
            }
            return encrypted;
        }

    }
}