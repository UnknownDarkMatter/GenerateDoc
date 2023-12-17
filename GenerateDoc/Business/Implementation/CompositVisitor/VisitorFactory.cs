using GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;
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
    private TextVisitor _textVisitor;
    private MarkDownVisitor _markDownVisitor;
    public VisitorFactory(TextVisitor textVisitor, MarkDownVisitor markDownVisitor)
    {
        _textVisitor = textVisitor ?? throw new ArgumentNullException(nameof(textVisitor));
        _markDownVisitor = markDownVisitor ?? throw new ArgumentNullException(nameof(markDownVisitor));
    }

    public ICompositeVisitor Create(DocumentationFormatEnum documentationFormatEnum)
    {
        switch(documentationFormatEnum)
        {
            case DocumentationFormatEnum.Text:
                {
                    return _textVisitor;
                }
            case DocumentationFormatEnum.MarkDown:
                {
                    return _markDownVisitor;
                }
            default:
                {
                    throw new NotImplementedException(documentationFormatEnum.ToString());
                }
        }
    }
}
