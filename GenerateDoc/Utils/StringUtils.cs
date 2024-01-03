using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Utils;

public static class StringUtils
{
    public static string DoPadLeft(this string input, int nbCars, char pad)
    {
        var sb = new StringBuilder();
        for(int i = 0; i < nbCars; i++)
        {
            sb.Append("&nbsp;");
        }
        sb.Append(input);
        return sb.ToString();
    }
}
