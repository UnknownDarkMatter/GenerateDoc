﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation.Parsing;

public class RuleElementParser : NamedElementParser
{
    public override string ElementName
    {
        get
        {
            return "Regle";
        }
    }
}
