﻿using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;

public class ScreenMarkDownVisitor : ICompositeVisitor
{
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
