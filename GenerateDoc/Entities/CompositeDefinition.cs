using GenerateDoc.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeDefinition
{
    public int Level
    {
        get
        {
            if (Parent == null)
            {
                return 0;
            }
            else
            {
                return Parent.Level + 1;
            }
        }
    }

    public CompositeDefinition Root
    {
        get { 
            return Parent == null ? this : Parent.Root; 
        }
    }

    public CompositeDefinition Parent { get; set; }

    public Guid Id { get; }
    public bool HasChildren { get; set; }
    public bool IsInsideAggregationGroup { get; set; }

    public CompositeDefinition(CompositeDefinition parent)
    {
        Parent = parent;
        Id = Guid.NewGuid();
    }

    public virtual CompositeDefinition Search(CompositeElement searched)
    {
        return null;
    }

    public int PaddingLevel()
    {
        if (Parent == null ||Level <= 2)
        {
            return 0;
        }
        else
        {
            if (Parent is CompositeCollection || Parent is CompositeAggregation)
            {
                return Parent.PaddingLevel() + 1;
            }
            else
            {
                return Parent.PaddingLevel();
            }
        }

    }

    public virtual void AcceptVisitor(ICompositeVisitor visitor)
    {
        visitor.VisitCompositeDefinition(this);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    private bool IsCollectionOfElementWithChilds(List<CompositeDefinition> collection)
    {
        var elementWithChilds = collection.FirstOrDefault(m => (m is CompositeElement e) && e.HasChildren) as CompositeElement;
        return elementWithChilds != null;
    }

}
