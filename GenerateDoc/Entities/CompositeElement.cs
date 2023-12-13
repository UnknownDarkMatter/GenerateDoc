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
    public string Description { get; set; }

    public CompositeElement(ElementTypeEnum elementType, string name, string description,
        CompositeDefinition parent):base(parent)
    {
        ElementType = elementType;
        Name = name;
        Description = description;
    }

    public override CompositeDefinition Search(CompositeElement searched)
    {
        if( searched.Name == this.Name && searched.ElementType == this.ElementType)
        {
            return this;
        }
        return null;
    }

    public override string ToString()
    {
        return $"{ElementType}-{Name}".PadLeft(Level * 3, ' ');
    }

}
