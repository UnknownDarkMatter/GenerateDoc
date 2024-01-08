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
display: flex;
align-items: flex-start;
margin-left:20px;
}
.content{
margin-left:20px;
}
.item{
margin-left:20px;
}
</style>
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"">
  <script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js""></script>
  <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js""></script>
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
            @"
<script>
$('.title').click(function(e){
    e.stopPropagation();
    $('.content', $(this).parent()).slideToggle();
});
</script>
"
            + $"</body>\r\n"
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
        if(collection.Children.Count>1)
        {
            if(collection.Children.First() is CompositeElement e)
            {
                var txt1 = BaseHtmlVisitor.GenerateWrappingStartingBlock(e.ElementDetails.Name);
                _fileContent.Append(txt1);
            }
            else
            {
                var txt1 = BaseHtmlVisitor.GenerateWrappingStartingBlock("");
                _fileContent.Append(txt1);
            }
        }

        foreach (var child in collection.Children)
        {
            child.AcceptVisitor(this);
        }

        if (collection.Children.Count > 1)
        {
            var txt2 = BaseHtmlVisitor.GenerateWrappingClosingBlock("");
            _fileContent.Append(txt2);
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
