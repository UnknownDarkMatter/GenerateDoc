using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Business.Implementation;

public class FileSearcher : IFileSearcher
{
    private readonly CommandLineOptions _commandLineOptions;
    private readonly IElementParser _elementParser;

    public FileSearcher(IElementParser elementParser, CommandLineOptions commandLineOptions)
    {
        _commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
        _elementParser = elementParser ?? throw new ArgumentNullException(nameof(elementParser));
    }

    public IEnumerable<CompositeDefinition> FindAll()
    {
        var occurencies = new List<CompositeDefinition>();
        var di = new DirectoryInfo(_commandLineOptions.SourceFolder);
        AddOccurencies(di, occurencies);
        return occurencies;
    }

    private void AddOccurencies(DirectoryInfo di, List<CompositeDefinition> occurencies)
    {
        List<FileInfo> files;
        if (_commandLineOptions.Pattern.Contains("|"))
        {
            var pattern = _commandLineOptions.Pattern.Split('|')
                .Select(p => p.Substring(p.IndexOf("."), p.Length - p.IndexOf(".")));
            files = di.GetFiles()
                .Where(f => pattern.Any(p => f.FullName.EndsWith(p)) )
                .ToList();
        }
        else
        {
            files = di.GetFiles(_commandLineOptions.Pattern).ToList();
        }
        foreach(var fi in files)
        {
            string fileContent = File.ReadAllText(fi.FullName);
            if (_elementParser.TryParseElement(fileContent, fi, 0, null, null, out CompositeDefinition element, out ElementDeclaration declaration))
            {
                element = element.Root;
                occurencies.Add(element);
            }
        }

        foreach (var diChild in di.GetDirectories())
        {
            AddOccurencies(diChild, occurencies);
        }
    }
}
