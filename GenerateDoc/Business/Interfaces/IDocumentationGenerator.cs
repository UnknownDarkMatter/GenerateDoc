using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Interfaces;

public interface IDocumentationGenerator
{
    public void GenerateDocumentation(DocumentationFormatEnum documentationFormatEnum);
}
