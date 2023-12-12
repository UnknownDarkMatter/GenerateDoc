using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.Parsing;

/// <summary>
/// Exemple de déclaration :
///      //@Ecran(Ecran 1):Begin
///      //@Ecran(Ecran 1.1):Begin
///      //@Ecran(Ecran 1.1.1):Begin
///      //@Ecran(Ecran 1.1.1):End
///      //@Ecran(Ecran 1.1.2):Begin
///      //@Ecran(Ecran 1.1.2.1):BeginAndEnd
///      //@Ecran(Ecran 1.1.2):End
///      //@Ecran(Ecran 1.1):End
///      //@Ecran(Ecran 1.2):BeginAndEnd
///      //@Ecran(Ecran 1):End
/// </summary>
public abstract class NamedElementParser : IElementParser
{
    public virtual string ElementName { get; set; }

    public bool TryParseElement(string fileContent, int start, int? end, CompositeDefinition parent,
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        end = end ?? fileContent.Length - 1;

        //fileContent = fileContent.Substring(start, (end ?? 0) + 1 - start);
        //if(fileContent.Length == 0)
        //{
        //    element = null;
        //    elementDeclaration = null;
        //    return false;
        //}
        //start = 0;
        //end = fileContent.Length - 1;

        var parseResultBeginSeparatedFromEnd = TryParseElementBeginSeparatedFromEnd(fileContent, start, end, parent,
            out CompositeDefinition elementTmpSeparatedFromEnd, out ElementDeclaration elementDeclarationTmpSeparatedFromEnd);

        var parseResultBeginAndEnd = TryParseElementBeginAndEnd(fileContent, start, end, parent,
            out CompositeDefinition elementTmpBeginAndEnd, out ElementDeclaration elementDeclarationTmpBeginAndEnd);

        if (parseResultBeginSeparatedFromEnd && parseResultBeginAndEnd)
        {
            if (elementDeclarationTmpBeginAndEnd.ElementStart < elementDeclarationTmpSeparatedFromEnd.ElementStart)
            {
                element = elementTmpBeginAndEnd;
                elementDeclaration = elementDeclarationTmpBeginAndEnd;
            }
            else
            {
                element = elementTmpSeparatedFromEnd;
                elementDeclaration = elementDeclarationTmpSeparatedFromEnd;
            }
            return true;
        }
        else if (parseResultBeginSeparatedFromEnd)
        {
            element = elementTmpSeparatedFromEnd;
            elementDeclaration = elementDeclarationTmpSeparatedFromEnd;
            return true;
        }
        else if (parseResultBeginAndEnd)
        {
            element = elementTmpBeginAndEnd;
            elementDeclaration = elementDeclarationTmpBeginAndEnd;
            return true;
        }

        element = null;
        elementDeclaration = null;
        return false;
    }

    public bool TryParseDeclaration(string fileContent, ref int start, int? end, out ElementDeclaration element)
    {
        throw new NotImplementedException();
    }

    public bool TryParseElementBeginAndEnd(string fileContent, int start, int? end, CompositeDefinition parent,
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        end = end ?? fileContent.Length - 1;
        if (end <= start)
        {
            element = null;
            elementDeclaration = null;
            return false;
        }

        var regex = new Regex($"//@(?<Type>{ElementName})\\((?<Name>[^\\)]+)\\):BeginAndEnd\\.");

        var match = regex.Match(fileContent);
        while (match.Success && (match.Index < start || match.Index >= end))
        {
            match = match.NextMatch();
        }
        if (match.Success)
        {
            var name = match.Groups["Name"].Value;
            var elementType = ElementTypeMapper.Map(match.Groups["Type"].Value);
            element = new CompositeElement(elementType, name, parent);

            int elementStart = match.Index;
            int? elementEnd = match.Index + match.Value.Length;
            string declarationContent = match.Value;
            string elementContent = match.Value;
            elementDeclaration = new ElementDeclaration(elementType, declarationContent, elementContent, elementStart, elementEnd, true);
            return true;
        }

        element = null;
        elementDeclaration = null;
        return false;
    }

    public bool TryParseElementBeginSeparatedFromEnd(string fileContent, int start, int? end, CompositeDefinition parent,
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        end = end ?? fileContent.Length - 1;
        if (end <= start)
        {
            element = null;
            elementDeclaration = null;
            return false;
        }

        var regexBegin = new Regex($"//@(?<Type>{ElementName})\\((?<Name>[^\\)]+)\\):Begin\\.");
        var matchBegin = regexBegin.Match(fileContent);
        while (matchBegin.Success && matchBegin.Index < start)
        {
            matchBegin = matchBegin.NextMatch();
        }
        if (matchBegin.Success)
        {
            var name = matchBegin.Groups["Name"].Value;

            var regexEnd = new Regex($"//@{ElementName}\\({name}\\):End\\.");
            var matchEnd = regexEnd.Match(fileContent);

            while (matchEnd.Success && matchEnd.Index >= end)
            {
                matchEnd = matchEnd.NextMatch();
            }
            if (matchEnd.Success)
            {
                var elementType = ElementTypeMapper.Map(matchBegin.Groups["Type"].Value);
                element = new CompositeElement(elementType, name, parent);

                string declarationContent = matchBegin.Value;
                int elementStart = matchBegin.Index;
                int? elementEnd = matchEnd.Index + matchEnd.Value.Length;
                string elementContent = fileContent.Substring(matchBegin.Index + matchBegin.Value.Length,
                    elementEnd.Value - (matchBegin.Index + matchBegin.Value.Length) - matchEnd.Value.Length);
                elementDeclaration = new ElementDeclaration(elementType, declarationContent, elementContent, elementStart, elementEnd, false);
                return true;
            }
        }

        element = null;
        elementDeclaration = null;
        return false;
    }

}
