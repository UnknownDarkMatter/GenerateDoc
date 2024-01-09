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
    public static int HtmlDomCount = 0;

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
        if (string.IsNullOrWhiteSpace(element.ElementDetails.Description))
        {
            return;
        }
        string title = string.IsNullOrWhiteSpace(element.ElementDetails.Description)
         ? element.ElementDetails.Name
         : element.ElementDetails.Description;
        //title = element.ElementDetails.Name;

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
        HtmlDomCount++;
        var result = $"<div class=\"wrapper\" id=\"{HtmlDomCount}\" >\r\n"
            + (
            string.IsNullOrWhiteSpace(title) 
            ? $"      <button  type=\"button\" class=\"title\" id=\"{HtmlDomCount + 1}\">+/-</button>\r\n"
            : $"      <button  type=\"button\" class=\"title\" id=\"{HtmlDomCount + 1}\">{title}</button>\r\n")
            +$"  <div class=\"content\" id=\"{HtmlDomCount+2}\"  class=\"collapse in\" >\r\n";
        HtmlDomCount++;
        HtmlDomCount++;
        return result;
    }

    public static string GenerateWrappingClosingBlock(string content)
    {
        return $"</div></div>\r\n";
    }

    public static string GenerateContentBlock(string content)
    {
        HtmlDomCount++;
        return $"<div class=\"item\" id=\"{HtmlDomCount}\">{content}</div>\r\n";
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
