using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace GenerateDoc.Infrastructure;

public class CommandLineOptions
{
    [Option('s', "src", Required = true, HelpText = "Set folder containing sources to documentate.")]
    public string SourceFolder { get; set; }

    [Option('o', "output", Required = true, HelpText = "Set folder where generate documentation.")]
    public string OutputFolder { get; set; }

    [Option('d', "devops", Required = true, HelpText = "Set base url with ending url encoded slash of devops url.")]
    public string DevOpsUrl { get; set; }

}
