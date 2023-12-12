using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation;

public class RecursiveElementParser : IElementParser
{
    private AnyElementParser _elementParser;
    public RecursiveElementParser(AnyElementParser elementParser)
    {
        _elementParser = elementParser ?? throw new ArgumentNullException(nameof(elementParser));
    }

    public bool TryParseElement(FileInfo file, int start, int? end, CompositeDefinition parent, 
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        var foundElements = new List<CompositeDefinition>();

        while (_elementParser.TryParseElement(file, start, end, parent, out CompositeDefinition currentElement, out ElementDeclaration currentDeclaration))
        {
            int childStart = start;
            int childEnd = childStart + currentDeclaration.ElementContent.Length;
            while (TryParseElement(file, childStart, childEnd, currentElement, out CompositeDefinition childElement, out ElementDeclaration childDeclaration))
            {
                if (currentElement is CompositeCollection collection)
                {
                    collection.Children.Add(childElement);
                }
                else
                {
                    var childElementCollection = new CompositeCollection(parent);
                    childElementCollection.Children.Add(childElement);
                    currentElement = childElementCollection;
                }

                childStart = childDeclaration.ElementStart + childDeclaration.ElementContent.Length;
                if (childStart>= childEnd) { break; }
            }

            foundElements.Add(currentElement);

            start = currentDeclaration.ElementStart + currentDeclaration.ElementContent.Length;
        }

        if (foundElements.Any())
        {
            if (foundElements.Count == 1)
            {
                element = foundElements[0];
            }
            else
            {
                var elementCollection = new CompositeCollection(parent);
                elementCollection.Children.AddRange(foundElements);
                element = elementCollection;
            }
            elementDeclaration = null;
            return true;
        }

        element = null;
        elementDeclaration = null;
        return false;
    }

    public bool TryParseDeclaration(string fileContent, ref int start, int? end, out ElementDeclaration element)
    {
        element = null;
        return false;
    }


}
