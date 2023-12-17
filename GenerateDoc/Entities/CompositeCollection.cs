using GenerateDoc.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Entities;

public class CompositeCollection : CompositeDefinition
{
    public List<CompositeDefinition> Children { get; set; }
    public CompositeCollection(CompositeDefinition parent) : base(parent)
    {
        Children = new List<CompositeDefinition>();
    }

    public override CompositeDefinition Search(CompositeElement searched)
    {
        foreach(var child in Children)
        {
            var found = child.Search(searched);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    public override void AcceptVisitor(ICompositeVisitor visitor)
    {
        visitor.VisitCompositeCollection(this);
    }

    public override string ToString()
    {
        string result = "[";
        foreach (var child in Children)
        {
            if(child is CompositeElement element)
            {
                result += $"{element.ElementDetails.Name}{(element.HasChildren ? "(has childs)" : "")},";
            }
            else
            {
                result += "(...),";
            }
        }
        result += "]";
        return result;
    }

}
