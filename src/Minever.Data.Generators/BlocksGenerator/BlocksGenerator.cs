using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Minever.Data.Core;
using Minever.Data.Generators.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Minever.Data.Generators;

[Generator]
public partial class BlocksGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var json   = context.AdditionalFiles.First().GetText(context.CancellationToken)!.ToString();
        //var json   = @"[ {""id"":0, ""name"": ""air""} ]";
        var blocks = JsonSerializer.Deserialize<Block[]>(json)!;

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($"using {nameof(Minever)}.{nameof(Data)}.{nameof(Core)};");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"namespace {nameof(Minever)}.{nameof(Data)}.Blocks;");
        sourceBuilder.AppendLine();

        foreach (var block in blocks)
        {
            sourceBuilder.AppendLine(GenerateBlockSourceCode(block));
            sourceBuilder.AppendLine();
        }

        context.AddSource("Blocks.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
            Debugger.Launch();
#endif
    }

    private string GenerateBlockSourceCode(Block block) =>
        $@"public class {block.Name.ToPascalCase()} : {nameof(IBlock)}
{{
    public int {nameof(IBlock.Id)} {{ get; }} = {block.Id};
    public string {nameof(IBlock.Name)} {{ get; }} = {block.Name};
}}";
}
