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

    public CompositeDefinition(CompositeDefinition parent)
    {
        Parent = parent;
        Id = Guid.NewGuid();
    }

    public virtual CompositeDefinition Search(CompositeElement searched)
    {
        return null;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
