using GenerateDoc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDoc.Utils;

public static class ElementTypeMapper
{
    public static ElementTypeEnum Map(string elementType)
    {
        switch(elementType)
        {
            case "Ecran": { return ElementTypeEnum.Screen; }
            case "Fonction": { return ElementTypeEnum.Function; }
            case "Regle": { return ElementTypeEnum.Rule; }
            case "Tag": { return ElementTypeEnum.Tag; }
            default:throw new NotImplementedException(elementType);
        }
    }
}
