using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Business.Implementation;

public class ScreenElementParser : NamedElementParser
{
    public override string ElementName {
        get
        {
            return "Ecran";
        } 
    }
}
