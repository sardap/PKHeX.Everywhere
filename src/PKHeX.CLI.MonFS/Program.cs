using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using PKHeX.Core;
using PKHeX.Facade;
using Spectre.Console;
using Spectre.Console.Cli;
using PKHeX.Facade.Repositories;
using PKHeX.MonFS;
using System.Text.Json;


namespace PKHeX.CLI;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MonFsCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MonFsCommand.Settings))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.ExplainCommand", "Spectre.Console.Cli")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.VersionCommand", "Spectre.Console.Cli")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.XmlDocCommand", "Spectre.Console.Cli")]
    public static void Main(string[] args)
    {
        var app = new CommandApp<MonFsCommand>();
        app.Run(args);
    }

}

public sealed class MonFsCommand : Command<MonFsCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        PrintHeader(settings);

        if (settings.SaveFilePath is null)
        {
            AnsiConsole.MarkupLine("[red]Please provide a save file path.[/]");
            return 1;
        }

        if (settings.PcFilePath is null)
        {
            AnsiConsole.MarkupLine("[red]Please provide a command.[/]");
            return 1;
        }

        return Run(settings.ResolveSaveFilePath(), settings);
    }

    private void PrintHeader(Settings settings)
    {
        AnsiConsole.Write(
            new FigletText("PKHeX MonFS CLI")
                .LeftJustified()
                .Color(Color.Red));

        if (settings.Version is null) return;

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[red]version {settings.Version?.ToString(3)}[/]");
        AnsiConsole.WriteLine();
    }

    private int Run(string path, Settings settings)
    {
        var game = Game.LoadFrom(path);

        AnsiConsole.MarkupLine(string.Empty);
        AnsiConsole.MarkupLine($"Successfully loaded the save state at: [blue]{path}[/]");
        AnsiConsole.MarkupLine(string.Empty);

        var pcPath = settings.ResolvePcFilePath();

        switch (settings.Command)
        {
            case Choices.Encode:
                {
                    var pc = JsonSerializer.Deserialize(File.ReadAllText(pcPath), PcJsonContext.Default.PC);
                    if (pc is null)
                        throw new Exception("PC guide file deserialization failed!");
                    Encode.Handle(game, pc);

                    File.WriteAllBytes(settings.ResolveSaveFilePath(), game.ToByteArray());
                    AnsiConsole.MarkupLine($"[green]Successfully overwritten file at {settings.ResolveSaveFilePath()}[/]");
                }
                break;
            case Choices.Decode:
                {
                    var pc = Decode.Handle(game);
                    File.WriteAllText(pcPath, JsonSerializer.Serialize(pc, PcJsonContext.Default.PC));
                }
                break;
            default:
                throw new Exception("Unknown command");
        }
        return 0;
    }

    private static class Choices
    {
        public const string Encode = "encode";
        public const string Decode = "decode";
    }

    public sealed class Settings : CommandSettings
    {
        [Description("The path to the save file.")]
        [CommandArgument(0, "[command]")]
        public string? Command { get; set; }

        [Description("The path to the save file.")]
        [CommandArgument(1, "[savefile]")]
        public string? SaveFilePath { get; set; }

        [Description("The path to the save file.")]
        [CommandArgument(2, "[pc_file]")]
        public string? PcFilePath { get; set; }

        public Version? Version => Assembly.GetExecutingAssembly().GetName().Version;

        public string ResolveSaveFilePath() => SaveFilePath;

        public string ResolvePcFilePath() => PcFilePath;
    }
}