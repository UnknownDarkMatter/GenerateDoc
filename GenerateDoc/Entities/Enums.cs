using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Entities
{
    public enum ElementTypeEnum
    {
        Undefined = 0,
        Screen = 1,
        Function = 2,
        Rule = 3,
        Tag = 4
    }

    public enum DocumentationFormatEnum
    {
        Text = 1,
        MarkDown = 2
    }
}
