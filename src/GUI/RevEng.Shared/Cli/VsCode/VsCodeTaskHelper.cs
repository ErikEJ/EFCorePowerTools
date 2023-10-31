using System.IO;
using System.Text;
using System.Text.Json;

namespace RevEng.Common.Cli.VsCode
{
    public static class VsCodeTaskHelper
    {
        public static void GenerateTaskPayload(string projectPath, int version, string redactedConnectionString)
        {
            var path = Path.Combine(projectPath, ".vscode");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, "tasks.json");

            if (File.Exists(path))
            {
                return;
            }

            var vsCodeTask = new VsCodeTask
            {
                version = "2.0.0",
                tasks = new System.Collections.Generic.List<TaskItem>
                {
                    new TaskItem
                    {
                        label = "EF Core Power Tools: Reverse Engineer",
                        command = "efcpt",
                        type = "shell",
                        args = new System.Collections.Generic.List<string>
                        {
                            "${input:connection}",
                            "mssql",
                        },
                        group = "none",
                        presentation = new Presentation
                        {
                            reveal = "always",
                        },
                        problemMatcher = "$msCompile",
                    },
                    new TaskItem
                    {
                        label = "EF Core Power Tools: Update",
                        command = "dotnet",
                        type = "shell",
                        args = new System.Collections.Generic.List<string>
                        {
                            "tool",
                            "update",
                            "-g",
                            "--version",
                            $"{version}.0.*-*",
                        },
                        group = "none",
                        presentation = new Presentation
                        {
                            reveal = "always",
                        },
                        problemMatcher = "$msCompile",
                    },
                },
                inputs = new System.Collections.Generic.List<Input>
                {
                    new Input
                    {
                        type = "promptString",
                        id = "connection",
                        description = "Enter the ADO.NET connection string to use",
                        @default = redactedConnectionString,
                    },
                },
            };

            File.WriteAllText(path, JsonSerializer.Serialize(vsCodeTask, new JsonSerializerOptions { WriteIndented = true }), Encoding.UTF8);
        }
    }
}
