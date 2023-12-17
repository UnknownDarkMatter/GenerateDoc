using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor;

public class VisitorFactory
{
    public ICompositeVisitor Create(DocumentationFormatEnum documentationFormatEnum)
    {
        switch(documentationFormatEnum)
        {
            case DocumentationFormatEnum.Text:
                {
                    return new TextVisitor();
                }
            case DocumentationFormatEnum.MarkDown:
                {
                    return new MarkDownVisitor();
                }
            default:
                {
                    throw new NotImplementedException(documentationFormatEnum.ToString());
                }
        }
    }
}
