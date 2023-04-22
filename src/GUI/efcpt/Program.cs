using CommandLine;
using Efcpt;

namespace ErikEJ.EfCorePowerTools;

public static class Program
{
    public static int Main(string[] args)
    {
        return Parser.Default.ParseArguments<ScaffoldOptions>(args)
          .MapResult(
            options => RunAndReturnExitCode(options),
            _ => 1);
    }

    private static int RunAndReturnExitCode(ScaffoldOptions options)
    {
        options.Dump();
        return 0;
    }
}
