using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation;

public class ScreenElementParser : IElementParser
{
    public bool TryParseElement(FileInfo file, int start, int? end, CompositeDefinition parent, 
        out CompositeDefinition element, out ElementDeclaration elementDeclaration)
    {
        throw new NotImplementedException();
    }

    public bool TryParseDeclaration(string fileContent, ref int start, int? end, out ElementDeclaration element)
    {
        throw new NotImplementedException();
    }
}
