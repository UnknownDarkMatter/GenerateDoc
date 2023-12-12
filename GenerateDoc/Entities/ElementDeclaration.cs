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
}
