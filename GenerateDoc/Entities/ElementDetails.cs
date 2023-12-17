using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class ElementDetails : IEquatable<ElementDetails>
{
    public ElementTypeEnum ElementType;

    public string Name;
    public string Description { get; set; }

    public ElementDetails(ElementTypeEnum elementType, string name, string description)
    {
        ElementType = elementType;
        Name = name;
        Description = description;
    }

    public bool Equals(ElementDetails? other)
    {
        return ElementType == other.ElementType && Name == other.Name;
    }

    public override string ToString()
    {
        return $"{ElementType}-{Name}{Description ?? ""}";
    }
}
