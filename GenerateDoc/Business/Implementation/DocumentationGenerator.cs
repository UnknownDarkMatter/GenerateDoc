using GenerateDoc.Business.Interfaces;
using GenerateDoc.Infrastructure;
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

    public DocumentationGenerator(IFileSearcher fileSearcher, ICompositeAggregator compositeAggregator)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
        _compositeAggregator = compositeAggregator;
    }

    public void GenerateDocumentation()
    {
        var elements = _fileSearcher.FindAll();
        elements = _compositeAggregator.Aggregate(elements);
    }
}
