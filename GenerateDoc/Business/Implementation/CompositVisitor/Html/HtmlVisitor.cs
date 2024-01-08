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

public class HtmlVisitor : ICompositeVisitor
{
    private TagHtmlVisitor _tagHtmlVisitor;
    private ScreenHtmlVisitor _screenHtmlVisitor;
    private RuleHtmlVisitor _ruleHtmlVisitor;
    private FunctionHtmlVisitor _functionHtmlVisitor;

    private CommandLineOptions _commandLineOptions;
    private FileContent _fileContent;
    private int _identation = Constants.Identation;

    public HtmlVisitor(TagHtmlVisitor tagHtmlVisitor, ScreenHtmlVisitor screenHtmlVisitor,
        RuleHtmlVisitor ruleHtmlVisitor, FunctionHtmlVisitor functionHtmlVisitor,
        CommandLineOptions commandLineOptions, FileContent fileContent)
    {
        _tagHtmlVisitor = tagHtmlVisitor ?? throw new ArgumentNullException(nameof(tagHtmlVisitor));
        _screenHtmlVisitor = screenHtmlVisitor ?? throw new ArgumentNullException(nameof(screenHtmlVisitor));
        _ruleHtmlVisitor = ruleHtmlVisitor ?? throw new ArgumentNullException(nameof(ruleHtmlVisitor));
        _functionHtmlVisitor = functionHtmlVisitor ?? throw new ArgumentNullException(nameof(functionHtmlVisitor));
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
        _fileContent = fileContent;
    }
    public void Initialize()
    {
        _fileContent.Append(
            @"<html>\r\n
<head>
<style>
.wrapper{
margin-left:30px;
}
.content{
}
.item{
}
</style>
</head>
"
            + $"<body>\r\n"
            + $"<h1>Documentation</h1>\r\n"
            );
    }
    public void Terminate()
    {
        string path = Path.Combine(_commandLineOptions.OutputFolder, "doc.html");

        _fileContent.Append(
            $"</body>\r\n"
            + $"<html>"
        );

        var fileContent = _fileContent.ToString();
        File.WriteAllText(path, fileContent);
    }

    public void VisitCompositeDefinition(dynamic definition)
    {
        if(definition is CompositeElement element)
        {
            VisitCompositeElement(element);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public void VisitCompositeAggregation(CompositeAggregation aggregation)
    {
        var txt = $"{aggregation.ElementDetails.Name}";

        txt = BaseHtmlVisitor.GenerateWrappingStartingBlock(txt) ;
        _fileContent.Append(txt);

        foreach (var aggregated in aggregation.Children)
        {
            VisitCompositeElement(aggregated.Key);

            foreach (var child in aggregated.Value)
            {
                child.AcceptVisitor(this);
            }
        }

        txt = BaseHtmlVisitor.GenerateWrappingClosingBlock(txt);
        _fileContent.Append(txt);

    }

    public void VisitCompositeCollection(CompositeCollection collection)
    {
        foreach (var child in collection.Children)
        {
            child.AcceptVisitor(this);
        }
    }

    public void VisitCompositeElement(CompositeElement element)
    {
        switch (element.ElementDetails.ElementType)
        {
            case ElementTypeEnum.Screen:
                {
                    _screenHtmlVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Function:
                {
                    _functionHtmlVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Rule:
                {
                    _ruleHtmlVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Tag:
                {
                    _tagHtmlVisitor.VisitCompositeElement(element);
                    break;
                }
            default:
                {
                    throw new NotImplementedException(element.ElementDetails.ElementType.ToString());
                }
        }
    }
}
