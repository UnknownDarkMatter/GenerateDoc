using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Utils;

public static class DocumentationFormatMapper
{
    public static DocumentationFormatEnum Map(string format)
    {
        switch (format)
        {
            case "txt": 
                {
                    return DocumentationFormatEnum.Text;
                }
            case "md":
                {
                    return DocumentationFormatEnum.MarkDown;
                }
            case "html":
                {
                    return DocumentationFormatEnum.Html;
                }
            default:
                {
                    throw new NotSupportedException(format);
                }
        }
    }
}
