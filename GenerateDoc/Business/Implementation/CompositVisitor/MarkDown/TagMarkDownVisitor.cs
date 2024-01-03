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

public class TagMarkDownVisitor : ICompositeVisitor
{
    private FileContent _fileContent;
    private readonly CommandLineOptions _commandLineOptions;
    private int _identation = Constants.Identation;
    public TagMarkDownVisitor(FileContent fileContent, CommandLineOptions commandLineOptions)
    {
        _fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
    }

    public void Initialize()
    {

    }
    public void Terminate()
    {

    }

    public void VisitCompositeElement(CompositeElement element)
    {
        var txt = $"{element.ElementDetails.ElementType}:{element.ElementDetails.Name})".ToString();

        txt += $" [src]({MarkdownHelper.GenerateSourceCodeHyperLink(element.SourceCodeDetails, _commandLineOptions)}";

        _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, ' ') + "\r\n");

        if (!string.IsNullOrWhiteSpace(element.ElementDetails.Description))
        {
            txt = $"  (Description : {element.ElementDetails.Description})".ToString();
            _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, ' ') + "\r\n");
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
