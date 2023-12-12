using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeElement : CompositeDefinition
{
    public ElementTypeEnum ElementType;
    public string Name;

    public CompositeElement(ElementTypeEnum elementType, string name,
        CompositeDefinition parent):base(parent)
    {
        ElementType = elementType;
        Name = name;
    }

    public override string ToString()
    {
        return $"{ElementType}-{Name}".PadLeft(Level * 3, ' ');
    }

}
