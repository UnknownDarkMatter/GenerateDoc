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

public class BaseMarkDownVisitor : ICompositeVisitor
{
    protected FileContent _fileContent;
    protected readonly CommandLineOptions _commandLineOptions;
    protected int _identation = Constants.Identation;

    public BaseMarkDownVisitor(FileContent fileContent, CommandLineOptions commandLineOptions)
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
        if (string.IsNullOrWhiteSpace(element.ElementDetails.Description))
        {
            return;
        }
        string title = string.IsNullOrWhiteSpace(element.ElementDetails.Description)
            ? element.ElementDetails.Name
            : element.ElementDetails.Description;

        var txt = $"{element.ElementDetails.ElementType}:{title}";

        txt += $" [src]({MarkdownHelper.GenerateSourceCodeHyperLink(element.SourceCodeDetails, _commandLineOptions)})";

        _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, Constants.PadMarkdown) + StringUtils.LineBreak());

        if (!string.IsNullOrWhiteSpace(element.ElementDetails.Description) && title != element.ElementDetails.Description)
        {
            txt = $"  (Description : {element.ElementDetails.Description})";
            _fileContent.Append(txt.DoPadLeft((element.PaddingLevel() + 1) * _identation, Constants.PadMarkdown) + StringUtils.LineBreak());
        }
    }

    public void VisitCompositeAggregation(CompositeAggregation aggregation)
    {
        throw new NotImplementedException();
    }

    public void VisitCompositeCollection(CompositeCollection collection)
    {
        throw new NotImplementedException();
    }

    public void VisitCompositeDefinition(dynamic definition)
    {
        throw new NotImplementedException();
    }
}
