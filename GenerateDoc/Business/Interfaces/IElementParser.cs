using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Interfaces;

public interface IElementParser
{
    public bool TryParseDeclaration(string fileContent, ref int start, int? end, out ElementDeclaration elementDeclaration);
    public bool TryParseElement(FileInfo file, int start, int? end, CompositeDefinition parent, out CompositeDefinition element,
        out ElementDeclaration elementDeclaration);
}
