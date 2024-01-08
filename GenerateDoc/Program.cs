using CommandLine;
using GenerateDoc.Business.Implementation;
using GenerateDoc.Business.Implementation.Aggregation;
using GenerateDoc.Business.Implementation.CompositVisitor;
using GenerateDoc.Business.Implementation.CompositVisitor.MarkDown;
using GenerateDoc.Business.Implementation.Parsing;
using GenerateDoc.Business.Interfaces;
using GenerateDoc.Entities;
using GenerateDoc.Infrastructure;
using GenerateDoc.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed<CommandLineOptions>(o =>
    {
        Console.WriteLine($"Source to documentate : \"{o.SourceFolder}\"");
        Console.WriteLine($"Output folder : \"{o.OutputFolder}\"");
        Console.WriteLine($"DevOps URL : \"{o.DevOpsUrl}\"");
        Console.WriteLine($"Format : \"{o.Format}\"");

        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<IElementParser>((serviceProvider) =>
        {
            return new RecursiveElementParser(new AnyElementParser(new List<IElementParser>()
            {
                new ScreenElementParser(),
                new FunctionElementParser(),
                new RuleElementParser(),
                new TagElementParser(),
            }));
        });
        builder.Services.AddSingleton<IFileSearcher, FileSearcher>();
        builder.Services.AddSingleton<IDocumentationGenerator, DocumentationGenerator>();
        builder.Services.AddSingleton<ICompositeAggregator, CompositeAggregator>();
        builder.Services.AddSingleton<VisitorFactory>();
        builder.Services.AddSingleton<TextVisitor>();
        builder.Services.AddSingleton<MarkDownVisitor>();
        builder.Services.AddSingleton<TagMarkDownVisitor>();
        builder.Services.AddSingleton<FunctionMarkDownVisitor> ();
        builder.Services.AddSingleton<RuleMarkDownVisitor> ();
        builder.Services.AddSingleton<ScreenMarkDownVisitor> ();
        builder.Services.AddSingleton<HtmlVisitor>();
        builder.Services.AddSingleton<TagHtmlVisitor>();
        builder.Services.AddSingleton<FunctionHtmlVisitor>();
        builder.Services.AddSingleton<RuleHtmlVisitor>();
        builder.Services.AddSingleton<ScreenHtmlVisitor>();
        builder.Services.AddSingleton<FileContent> ();
        builder.Services.AddSingleton((serviceProvider) => { return o; });

        using IHost host = builder.Build();
        var documentationGenerator = host.Services.GetRequiredService<IDocumentationGenerator>();
        documentationGenerator.GenerateDocumentation(DocumentationFormatMapper.Map(o.Format));

        host.Run();
    });
