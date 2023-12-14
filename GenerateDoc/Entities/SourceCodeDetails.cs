using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class SourceCodeDetails
{
    public FileInfo File { get; set; }
    public int Line { get; set; }

    public SourceCodeDetails(FileInfo file, int line)
    {
        File = file;
        Line = line;
    }
}
