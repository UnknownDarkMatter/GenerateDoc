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

public class MarkDownVisitor : ICompositeVisitor
{
    private TagMarkDownVisitor _tagMarkDownVisitor;
    private ScreenMarkDownVisitor _screenMarkDownVisitor;
    private RuleMarkDownVisitor _ruleMarkDownVisitor;
    private FunctionMarkDownVisitor _functionMarkDownVisitor;

    private CommandLineOptions _commandLineOptions;
    private FileContent _fileContent;
    private int _identation = Constants.Identation;

    public MarkDownVisitor(TagMarkDownVisitor tagMarkDownVisitor, ScreenMarkDownVisitor screenMarkDownVisitor,
        RuleMarkDownVisitor ruleMarkDownVisitor, FunctionMarkDownVisitor functionMarkDownVisitor,
        CommandLineOptions commandLineOptions, FileContent fileContent)
    {
        _tagMarkDownVisitor = tagMarkDownVisitor ?? throw new ArgumentNullException(nameof(tagMarkDownVisitor));
        _screenMarkDownVisitor = screenMarkDownVisitor ?? throw new ArgumentNullException(nameof(screenMarkDownVisitor));
        _ruleMarkDownVisitor = ruleMarkDownVisitor ?? throw new ArgumentNullException(nameof(ruleMarkDownVisitor));
        _functionMarkDownVisitor = functionMarkDownVisitor ?? throw new ArgumentNullException(nameof(functionMarkDownVisitor));
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
        _fileContent = fileContent;
    }
    public void Initialize()
    {

    }
    public void Terminate()
    {
        string path = Path.Combine(_commandLineOptions.OutputFolder, "doc.md");
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
        var txt = $"Group:{aggregation.ElementDetails.Name}".ToString();
        _fileContent.Append(txt.DoPadLeft((aggregation.PaddingLevel()) * _identation, ' ') + "\r\n");
        foreach (var aggregated in aggregation.Children)
        {
            txt = $"-Group child : {aggregated.Key.ElementDetails.ElementType}:{aggregated.Key.ElementDetails.Name}".ToString();
            _fileContent.Append(txt.DoPadLeft((aggregated.Key.PaddingLevel()) * _identation, ' ') + "\r\n");

            if (!string.IsNullOrWhiteSpace(aggregated.Key.ElementDetails.Description))
            {
                txt = $"  (Description : {aggregated.Key.ElementDetails.Description})".ToString();
                _fileContent.Append(txt.DoPadLeft((aggregated.Key.PaddingLevel() + 1) * _identation, ' ') + "\r\n");
            }

            foreach (var child in aggregated.Value)
            {
                child.AcceptVisitor(this);
            }
        }
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
                    _screenMarkDownVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Function:
                {
                    _functionMarkDownVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Rule:
                {
                    _ruleMarkDownVisitor.VisitCompositeElement(element);
                    break;
                }
            case ElementTypeEnum.Tag:
                {
                    _tagMarkDownVisitor.VisitCompositeElement(element);
                    break;
                }
            default:
                {
                    throw new NotImplementedException(element.ElementDetails.ElementType.ToString());
                }
        }
    }
}
