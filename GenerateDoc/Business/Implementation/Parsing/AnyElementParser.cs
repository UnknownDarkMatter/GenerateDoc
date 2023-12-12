using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.Parsing;

public class AnyElementParser : IElementParser
{
    private IEnumerable<IElementParser> _allElementParsers;
    public AnyElementParser(IEnumerable<IElementParser> allElementParsers)
    {
        _allElementParsers = allElementParsers ?? throw new ArgumentNullException(nameof(allElementParsers));
    }

    public bool TryParseElement(string fileContent, int start, int? end, CompositeDefinition parent,
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        var elements = new List<CompositeDefinition>();
        var declarations = new List<ElementDeclaration>();
        foreach (var parser in _allElementParsers)
        {
            if (parser.TryParseElement(fileContent, start, end, parent,
                out CompositeDefinition elementTmp, out ElementDeclaration elementDeclarationTmp))
            {
                elements.Add(elementTmp);
                declarations.Add(elementDeclarationTmp);
            }
        }
        if (declarations.Any())
        {
            int index = -1;
            int minPosition = int.MaxValue;
            for (int i = 0; i < declarations.Count; i++)
            {
                var declaration = declarations[i];
                if (declaration.ElementStart < minPosition)
                {
                    index = i;
                    minPosition = declaration.ElementStart;
                }
            }
            element = elements[index];
            elementDeclaration = declarations[index];
            return true;
        }

        element = null;
        elementDeclaration = null;
        return false;
    }

    public bool TryParseDeclaration(string fileContent, ref int start, int? end, out ElementDeclaration element)
    {
        foreach (var parser in _allElementParsers)
        {
            var startInitial = start;
            if (parser.TryParseDeclaration(fileContent, ref startInitial, end, out element))
            {
                start = startInitial;
                return true;
            }
        }

        element = null;
        return false;
    }


}
