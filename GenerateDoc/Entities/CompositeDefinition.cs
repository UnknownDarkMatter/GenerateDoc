using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities;

public class CompositeDefinition
{
    public CompositeDefinition Parent { get; set; }

    public CompositeDefinition(CompositeDefinition parent)
    {
        Parent = parent;
    }
}
