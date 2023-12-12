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
    private readonly CommandLineOptions _commandLineOptions;

    public DocumentationGenerator(CommandLineOptions commandLineOptions)
    {
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
    }

    public void GenerateDocumentation()
    {

    }
}
