using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.Parsing;

public class RecursiveElementParser : IElementParser
{
    private AnyElementParser _elementParser;
    public RecursiveElementParser(AnyElementParser elementParser)
    {
        _elementParser = elementParser ?? throw new ArgumentNullException(nameof(elementParser));
    }

    public bool TryParseElement(string fileContent, FileInfo fi, int start, int? end, CompositeDefinition parent,
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        var foundElements = new List<CompositeDefinition>();
        var foundDeclarations = new List<ElementDeclaration>();

        //for each sibblings occurencies
        while (_elementParser.TryParseElement(fileContent, fi, start, end, parent, out CompositeDefinition currentElement, out ElementDeclaration currentDeclaration))
        {
            if (currentDeclaration.IsBeginAndEnd)
            {
                foundElements.Add(currentElement);
                foundDeclarations.Add(currentDeclaration);
                start = currentDeclaration.ElementEnd.Value;
                continue;
            }

            int childStart = currentDeclaration.ElementStart + currentDeclaration.DeclarationContent.Length;
            int childEnd = currentDeclaration.ElementEnd.Value;

            //foreach nested occurencies (deepth)
            while (TryParseElement(fileContent, fi, childStart, childEnd, currentElement, out CompositeDefinition childElement, out ElementDeclaration childDeclaration))
            {
                if(currentElement.Parent is null)
                {
                    var rootCollection = new CompositeCollection(null);
                    rootCollection.Children.Add(currentElement);
                    currentElement.Parent = rootCollection;
                    var nestedElementCollection = new  CompositeCollection(rootCollection);
                    rootCollection.Children.Add(nestedElementCollection);
                    currentElement = nestedElementCollection;
                }
                if (currentElement is CompositeCollection collection)
                {
                    collection.Children.Add(childElement);
                }
                else
                {
                    var currentElementCollection = new CompositeCollection(currentElement.Parent);
                    currentElementCollection.Children.Add(currentElement);
                    currentElement.Parent = currentElementCollection;

                    var childElementCollection = new CompositeCollection(currentElementCollection);
                    childElementCollection.Children.Add(childElement);
                    childElement.Parent = childElementCollection;

                    currentElementCollection.Children.Add(childElementCollection);

                    currentElement = currentElementCollection;
                }

                childStart = childDeclaration.ElementEnd.Value;
            }

            foundElements.Add(currentElement);
            foundDeclarations.Add(currentDeclaration);

            start = currentDeclaration.ElementEnd ?? currentDeclaration.ElementStart + currentDeclaration.DeclarationContent.Length;
        }

        if (foundElements.Any())
        {
            if (foundElements.Count == 1)
            {
                element = foundElements[0];
                elementDeclaration = foundDeclarations[0];
            }
            else
            {
                var elementCollection = new CompositeCollection(parent);
                elementCollection.Children.AddRange(foundElements);
                element = elementCollection;

                ElementTypeEnum elementType = ElementTypeEnum.Undefined;
                string declarationContent = foundDeclarations[0].DeclarationContent;
                int elementStart = foundDeclarations.Min(m => m.ElementStart);
                int elementEnd = foundDeclarations.Max(m => m.ElementEnd.Value);
                string elementContent = fileContent.Substring(elementStart, elementEnd - elementStart + 1);
                bool isBeginAndEnd = false;
                elementDeclaration = new ElementDeclaration(elementType, declarationContent, elementContent,
                    elementStart, elementEnd, isBeginAndEnd);
            }
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
