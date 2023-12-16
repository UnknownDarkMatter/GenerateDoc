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
    public Dictionary<CompositeElement, List<CompositeDefinition>> Children { get; set; }
    
    public CompositeAggregation(ElementDetails elementDetails,
        CompositeDefinition parent) : base(parent)
    {
        ElementDetails = elementDetails;
        Children = new Dictionary<CompositeElement, List<CompositeDefinition>>();
    }

    public override CompositeDefinition Search(CompositeElement searched)
    {
        if (searched.ElementDetails.Equals(this.ElementDetails))
        {
            return this;
        }
        foreach (var list in Children.Values)
        {
            foreach (var child in list)
            {
                var found = child.Search(searched);
                if (found != null) return found;
            }
        }

        return null;
    }

    public override string ToString()
    {
        return $"Group : {ElementDetails}";
    }

}
