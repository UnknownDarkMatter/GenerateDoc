using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Interfaces;

public interface ICompositeVisitor
{
    public void Initialize();
    public void Terminate();
    public void VisitCompositeDefinition(CompositeDefinition definition);
    public void VisitCompositeElement(CompositeElement element);
    public void VisitCompositeCollection(CompositeCollection collection);
    public void VisitCompositeAggregation(CompositeAggregation aggregation);
}
