using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;

public class MarkDownVisitor : ICompositeVisitor
{
    private TagMarkDownVisitor _tagMarkDownVisitor;
    private ScreenMarkDownVisitor _screenMarkDownVisitor;
    private RuleMarkDownVisitor _ruleMarkDownVisitor;
    private FunctionMarkDownVisitor _functionMarkDownVisitor;
    public MarkDownVisitor(TagMarkDownVisitor tagMarkDownVisitor, ScreenMarkDownVisitor screenMarkDownVisitor,
        RuleMarkDownVisitor ruleMarkDownVisitor, FunctionMarkDownVisitor functionMarkDownVisitor)
    {
        _tagMarkDownVisitor = tagMarkDownVisitor ?? throw new ArgumentNullException(nameof(tagMarkDownVisitor));
        _screenMarkDownVisitor = screenMarkDownVisitor ?? throw new ArgumentNullException(nameof(screenMarkDownVisitor));
        _ruleMarkDownVisitor = ruleMarkDownVisitor ?? throw new ArgumentNullException(nameof(ruleMarkDownVisitor));
        _functionMarkDownVisitor = functionMarkDownVisitor ?? throw new ArgumentNullException(nameof(functionMarkDownVisitor));
    }
    public void Initialize()
    {

    }
    public void Terminate()
    {

    }

    public void VisitCompositeDefinition(CompositeDefinition definition)
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

    public void VisitCompositeElement(CompositeElement element)
    {
        throw new NotImplementedException();
    }
}
