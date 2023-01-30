using System.Text.RegularExpressions;
using Test;

//var f = Caesar.FormatMessage("ab ^ c | z");
//var e = Caesar.Encrypt(f, 15);
//var d = Caesar.Decrypt(e, 15);
//Console.WriteLine(f);
//Console.WriteLine(e);
//Console.WriteLine(d);




var sf1 = Caesar.FormatMessage(File.ReadAllText("song 1.txt"));
var se1 = Caesar.Encrypt(sf1, 4);
Console.WriteLine(se1);
//Console.WriteLine(Caesar.Decrypt(se1, 4));

Console.WriteLine();

var sf2 = Caesar.FormatMessage(File.ReadAllText("song 2.txt"));
var se2 = Caesar.Encrypt(sf2, 15);
Console.WriteLine(se2);
//Console.WriteLine(Caesar.Decrypt(se2,15));


namespace Test
{
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