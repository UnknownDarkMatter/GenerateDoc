using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Utils;

public static class StringUtils
{
    public static string DoPadLeft(this string input, int nbCars, string pad)
    {
        var sb = new StringBuilder();
        for(int i = 0; i < nbCars; i++)
        {
            sb.Append(pad);
        }
        sb.Append(input);
        return sb.ToString();
    }

    public static string LineBreak()
    {
        return "\r\n\r\n";
    }
}
