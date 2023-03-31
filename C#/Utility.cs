using System;
using System.IO;
using System.Security.Cryptography;

class Utility
{
    public const string Tab = "\t";

    public static string ComputeFileHash(string FileName)
    {
        return BitConverter.ToString(
            new MD5CryptoServiceProvider().ComputeHash(
                new FileStream(FileName, FileMode.Open, FileAccess.Read))).Replace("-", "").ToLower();
    }
}
