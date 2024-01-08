using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using GenerateDoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;

public class RuleHtmlVisitor : BaseHtmlVisitor
{
    public RuleHtmlVisitor(FileContent fileContent, CommandLineOptions commandLineOptions): base(fileContent, commandLineOptions)
    {
    }

    public void Initialize()
    {

    }
    public void Terminate()
    {

    }

    public void VisitCompositeElement(CompositeElement element)
    {
        base.VisitCompositeElement(element);
    }

    public void VisitCompositeDefinition(dynamic definition)
    {
        throw new NotImplementedException();
    }
    public void VisitCompositeAggregation(CompositeAggregation aggregation)
    {
        throw new NotImplementedException();
    }

    public void VisitCompositeCollection(CompositeCollection collection)
    {
        throw new NotImplementedException();
    }

}
