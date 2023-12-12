using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateDoc.Entities;

public class CompositeCollection : CompositeDefinition
{
    public List<CompositeDefinition> Children { get; set; }
    public CompositeCollection(CompositeDefinition parent) : base(parent)
    {
        Children = new List<CompositeDefinition>();
    }
    public override string ToString()
    {
        string result = "";
        //foreach(var child in Children)
        //{
        //    result += $"{child}\r\n".PadLeft(Level * 3, ' ');
        //}
        result = $"[+] {((Parent is null) ? "" : Parent.ToString())}".PadLeft(Level + 1, ' ');
        return result;
    }

}
