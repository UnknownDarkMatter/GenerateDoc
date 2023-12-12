using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeCollection : CompositeDefinition
{
    public List<CompositeDefinition> Children { get; set; }
    public CompositeCollection(CompositeDefinition parent) : base(parent)
    {
        Children = new List<CompositeDefinition>();
    }

}
