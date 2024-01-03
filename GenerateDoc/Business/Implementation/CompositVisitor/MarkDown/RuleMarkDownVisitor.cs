using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;

public class RuleMarkDownVisitor : ICompositeVisitor
{
    private FileContent _fileContent;
    private int _identation = Constants.Identation;
    public RuleMarkDownVisitor(FileContent fileContent)
    {
        _fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
    }

    public void Initialize()
    {

    }
    public void Terminate()
    {

    }

    public void VisitCompositeElement(CompositeElement element)
    {
        var txt = $"{element.ElementDetails.ElementType}:{element.ElementDetails.Name}".ToString();
        _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, ' ') + StringUtils.LineBreak());
        if (!string.IsNullOrWhiteSpace(element.ElementDetails.Description))
        {
            txt = $"  (Description : {element.ElementDetails.Description})".ToString();
            _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, ' ') + StringUtils.LineBreak());
        }
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
