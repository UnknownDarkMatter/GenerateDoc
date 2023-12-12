using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeElement : CompositeDefinition
{
    public ElementTypeEnum ElementType;
    public CompositeElement(ElementTypeEnum elementType, CompositeDefinition parent):base(parent)
    {
        ElementType = elementType;
    }

}
