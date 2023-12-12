using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class ElementDeclaration
{
    public ElementTypeEnum ElementType { get; set; }
    public string DeclarationContent { get; set; }
    public string ElementContent { get; set; }
    public int ElementStart { get; set; }
    public int? ElementEnd { get; set; }
    public bool IsBeginAndEnd { get; set; }
        
    public ElementDeclaration(ElementTypeEnum elementType, string declarationContent, 
        string elementContent, int elementStart, int? elementEnd, bool isBeginAndEnd)
    {
        ElementType = elementType;
        DeclarationContent = declarationContent;
        ElementContent = elementContent;
        ElementStart = elementStart;
        ElementEnd = elementEnd;
        IsBeginAndEnd = isBeginAndEnd;
    }
}
