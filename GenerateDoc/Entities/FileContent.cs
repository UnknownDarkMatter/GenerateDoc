using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class FileContent
{
    private StringBuilder _sb = new StringBuilder();

    public void Append(string text)
    {
        _sb.Append(text);
    }

    public override string ToString()
    {
        return _sb.ToString();
    }
}
