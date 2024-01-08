using GenerateDoc.Business.Implementation.CompositVisitor;
using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using GenerateDoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation;

public class DocumentationGenerator : IDocumentationGenerator
{
    private readonly IFileSearcher _fileSearcher;
    private readonly ICompositeAggregator _compositeAggregator;
    private readonly VisitorFactory _visitorFactory;
    private readonly FileContent _fileContent;

    public DocumentationGenerator(IFileSearcher fileSearcher, ICompositeAggregator compositeAggregator,
        VisitorFactory visitorFactory, FileContent fileContent)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _compositeAggregator = compositeAggregator ?? throw new ArgumentNullException(nameof(compositeAggregator));
        _visitorFactory = visitorFactory ?? throw new ArgumentNullException(nameof(visitorFactory));
        _fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
    }

    public void GenerateDocumentation(DocumentationFormatEnum documentationFormatEnum)
    {
        Console.WriteLine($"########### DEBUT #########");
        var elements = _fileSearcher.FindAll();
        var elementsAggregated = _compositeAggregator.Aggregate(elements);
        var visitor = _visitorFactory.Create(documentationFormatEnum);

        visitor.Initialize();
        elementsAggregated.AcceptVisitor(visitor);
        visitor.Terminate();
        Console.WriteLine($"########### FIN #########");
    }
}
