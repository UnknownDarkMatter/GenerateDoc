using CommandLine;
using GenerateDoc.Business.Implementation;
using GenerateDoc.Business.Interfaces;
using GenerateDoc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed<CommandLineOptions>(o =>
    {
        Console.WriteLine($"Source to documentate : \"{o.SourceFolder}\"");
        Console.WriteLine($"Output folder : \"{o.OutputFolder}\"");
        Console.WriteLine($"DevOps URL : \"{o.DevOpsUrl}\"");

        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<IElementParser>((serviceProvider) =>
        {
            return new RecursiveElementParser(new AnyElementParser(new List<IElementParser>()
            {
                new ScreenElementParser()
            }));
        });
        builder.Services.AddSingleton<IFileSearcher, FileSearcher>();
        builder.Services.AddSingleton<IDocumentationGenerator, DocumentationGenerator>();
        builder.Services.AddSingleton((serviceProvider) => { return o; });

        using IHost host = builder.Build();
        var documentationGenerator = host.Services.GetRequiredService<IDocumentationGenerator>();
        documentationGenerator.GenerateDocumentation();

        host.Run();
    });
