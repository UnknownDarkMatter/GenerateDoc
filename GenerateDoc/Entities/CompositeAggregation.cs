using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Entities;

public class CompositeAggregation : CompositeDefinition
{
    public ElementDetails ElementDetails { get; set; }
    public Dictionary<SourceCodeDetails, List<CompositeDefinition>> Children { get; set; }
    
    public CompositeAggregation(ElementTypeEnum elementType, string name, string description,
        CompositeDefinition parent) : base(parent)
    {
        ElementDetails = new ElementDetails(elementType, name, description);
        Children = new Dictionary<SourceCodeDetails, List<CompositeDefinition>>();
    }

    public override CompositeDefinition Search(CompositeElement searched)
    {
        if (searched.ElementDetails.Equals(this.ElementDetails))
        {
            return this;
        }
        return null;
    }

    public override string ToString()
    {
        return $"Group : {ElementDetails}";
    }

}
