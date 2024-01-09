using GenerateDoc.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeElement : CompositeDefinition
{

    public ElementDetails ElementDetails { get; set; }
    public SourceCodeDetails SourceCodeDetails { get; set; }

    public CompositeElement(ElementTypeEnum elementType, string name, string description,
        FileInfo file, int line,
        CompositeDefinition parent):base(parent)
    {
        ElementDetails = new ElementDetails(elementType, name, description);
        SourceCodeDetails = new SourceCodeDetails(file, line);
    }

    public override CompositeDefinition Search(CompositeElement searched)
    {
        if (searched.ElementDetails.Equals(this.ElementDetails))
        {
            return this;
        }
        return null;
    }

    public override void AcceptVisitor(ICompositeVisitor visitor)
    {
        visitor.VisitCompositeElement(this);
    }

    public override string ToString()
    {
        return $"{ElementDetails}{(HasChildren ? "(has childs)" : "")}".PadLeft(Level * 3, ' ');
    }

    public override bool ContainsCompositeElement()
    {
        return true;
    }

}
