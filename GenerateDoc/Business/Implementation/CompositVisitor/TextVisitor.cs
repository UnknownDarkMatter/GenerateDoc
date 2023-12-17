using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using GenerateDoc.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Business.Implementation.CompositVisitor;

public class TextVisitor : ICompositeVisitor
{
    private CommandLineOptions _commandLineOptions;
    private StringBuilder _sb;
    private int _identation = 2;
    public TextVisitor(CommandLineOptions commandLineOptions)
    {
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
    }
    public void Initialize()
    {
        _sb = new StringBuilder();
    }
    public void Terminate()
    {
        string path = Path.Combine(_commandLineOptions.OutputFolder, "doc.txt");
        var fileContent = _sb.ToString();
        File.WriteAllText(path, fileContent);
    }

    public void VisitCompositeDefinition(CompositeDefinition definition)
    {
        throw new NotImplementedException();
    }

    public void VisitCompositeAggregation(CompositeAggregation aggregation)
    {
        var txt = $"Group:{aggregation.ElementDetails.Name}".ToString();
        _sb.Append(txt.DoPadLeft((aggregation.PaddingLevel() - 1) * _identation, ' ') + "\r\n");
        foreach (var aggregated in aggregation.Children)
        {
            //txt = $"-{aggregated.Key.ElementDetails.ElementType}".ToString();
            //_sb.Append(txt.DoPadLeft((aggregated.Key.PaddingLevel() - 1) * _identation, ' ') + "\r\n");
            foreach(var child in aggregated.Value)
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
        var txt = $"{element.ElementDetails.ElementType}:{element.ElementDetails.Name}".ToString();
        _sb.Append(txt.DoPadLeft(element.PaddingLevel() * _identation, ' ') + "\r\n");
        if (!string.IsNullOrWhiteSpace(element.ElementDetails.Description))
        {
            txt = $"   (Description : {element.ElementDetails.Description})".ToString();
            _sb.Append(txt.DoPadLeft(element.PaddingLevel() * _identation, ' ') + "\r\n");
        }
    }
}
