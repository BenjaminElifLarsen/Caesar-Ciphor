using System.Text.RegularExpressions;

var f = Caesar.FormatMessage("ab ^ c | z");
var e = Caesar.Encrypt(f, 23);
var d = Caesar.Decrypt(e, 23);
Console.WriteLine(f);
Console.WriteLine(e);
Console.WriteLine(d);


public static class Caesar
{
    public static string FormatMessage(string message)
    {
        Regex regex = new("[\\w ]");
        var formatted = "";
        foreach(var c in message.ToUpper())
        {
            if (regex.IsMatch(c.ToString()))
            {
                formatted += (char)c;
            }
            //if(formatted.Last() == ' ')
            //{
            //    formatted.Remove(formatted.Length- 1, 1);
            //}
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
                else if(ce < 65)
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