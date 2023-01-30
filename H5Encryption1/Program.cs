﻿using System.Text.RegularExpressions;
using Test;

//var f = Caesar.FormatMessage("ab ^ c | z");
//var e = Caesar.Encrypt(f, 15);
//var d = Caesar.Decrypt(e, 15);
//Console.WriteLine(f);
//Console.WriteLine(e);
//Console.WriteLine(d);




var sf1 = Caesar.FormatMessage(File.ReadAllText("song 1.txt"));
var se1 = Caesar.Encrypt(sf1, 4);
//Console.WriteLine(se1);
//Console.WriteLine(Caesar.Decrypt(se1, 4));

Console.WriteLine();

var sf2 = Caesar.FormatMessage(File.ReadAllText("song 2.txt"));
var se2 = Caesar.Encrypt(sf2, 15);
//Console.WriteLine(se2);
//Console.WriteLine(Caesar.Decrypt(se2,15));


//var brutes = CaesarAnalysis.BruteForce(se1);
//foreach(var t in brutes.Keys)
//{
//    Console.WriteLine($"{t}: " + brutes[t]);
//    Console.WriteLine();
//}

var text = se2;
var keys = CaesarAnalysis.FindPossibleKeys(text);
var key = keys.OrderByDescending(x => x.Value).First().Key;
Console.WriteLine($"{key}: " + Caesar.Decrypt(text, key));



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

        public static Dictionary<byte, double> FindPossibleKeys(string decrypted)
        {
            var freqNorm = new double[]
            { //letters, A-Z
                0.64297, 0.11746, 0.21902, 0.33483, 1.00000, 0.17541,
                0.15864, 0.47977, 0.54842, 0.01205, 0.06078, 0.31688, 0.18942,
                0.53133, 0.59101, 0.15187, 0.00748, 0.47134, 0.49811, 0.71296,
                0.21713, 0.07700, 0.18580, 0.01181, 0.15541, 0.00583
            };
            var noSpace = decrypted.Where(c => c != ' ');
            var groupping = noSpace.GroupBy(x => x);
            var mostCommon = groupping.OrderByDescending(x => x.Count()).First().Key;
            var difference = (byte)(mostCommon - 'E');

            var orderGroupping = groupping.OrderBy(x => x.Key).ToArray();
            var differences = new List<byte>();
            var dic = new Dictionary<byte, double>();
            foreach(var key in orderGroupping)
            {
                var alphabetDifference = (byte)(key.Key - 'A');
                var chosenFreq = freqNorm[alphabetDifference];
                dic.Add(alphabetDifference, chosenFreq);
            }

            return dic;
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
            if (key == 0 || key >= 26)
                throw new Exception();
            return Coder(message, key);
        }

        public static string Decrypt(string message, byte key)
        {
            if (key == 0 || key >= 26)
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