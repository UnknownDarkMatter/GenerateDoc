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

    public DocumentationGenerator(IFileSearcher fileSearcher)
    {
        _fileSearcher = fileSearcher ?? throw new ArgumentNullException(nameof(fileSearcher));
    }

    public void GenerateDocumentation()
    {
        var elements = _fileSearcher.FindAll();
    }
}
