using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;

public static class MarkdownHelper
{
    public static string GenerateSourceCodeHyperLink(SourceCodeDetails sourceCodeDetails, CommandLineOptions commandLineOptions)
    {
        string hyperLink = commandLineOptions.DevOpsUrl;
        string path = sourceCodeDetails.File.FullName.Substring(commandLineOptions.SourceFolder.Length);
        path = path.Replace("/", "\\");
        if (!path.StartsWith("\\"))
        {
            path = "\\" + path;
        }
        return hyperLink + path;
    }

}
