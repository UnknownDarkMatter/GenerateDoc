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

public class BaseHtmlVisitor : ICompositeVisitor
{
    protected FileContent _fileContent;
    protected readonly CommandLineOptions _commandLineOptions;
    protected int _identation = Constants.Identation;

    public BaseHtmlVisitor(FileContent fileContent, CommandLineOptions commandLineOptions)
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
        string title = string.IsNullOrWhiteSpace(element.ElementDetails.Description)
         ? element.ElementDetails.Name
         : element.ElementDetails.Description;
        title = element.ElementDetails.Name;

        var txt = $"{element.ElementDetails.ElementType}:{title}";

        txt += $" <a href=\"{MarkdownHelper.GenerateSourceCodeHyperLink(element.SourceCodeDetails, _commandLineOptions)}\">src</a>";

        string additionnalDescription = "";
        if (!string.IsNullOrWhiteSpace(element.ElementDetails.Description) && title != element.ElementDetails.Description)
        {
            additionnalDescription = $"<br/>(Description : {element.ElementDetails.Description})";
        }

        txt = GenerateContentBlock(txt + additionnalDescription);

        _fileContent.Append(txt);

    }

    public static string GenerateWrappingStartingBlock(string title)
    {
        return $"<div class=\"wrapper\">\r\n" +
            $"  <div class=\"content\">\r\n" +
            $"      <div class=\"title\">{title}\r\n" +
            $"      </div>\r\n";
    }

    public static string GenerateWrappingClosingBlock(string content)
    {
        return $"</div></div>\r\n";
    }

    public static string GenerateContentBlock(string content)
    {
        return $"<div class=\"item\">{content}</div>\r\n";
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
