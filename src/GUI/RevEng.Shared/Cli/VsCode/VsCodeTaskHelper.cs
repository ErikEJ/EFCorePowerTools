using System.IO;
using System.Text;
using System.Text.Json;

namespace RevEng.Common.Cli.VsCode
{
    public static class VsCodeTaskHelper
    {
        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            WriteIndented = true,
        };

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
                        label = "EF Core Power Tools: Edit Configuration",
                        command = "code",
                        type = "shell",
                        args = new System.Collections.Generic.List<string>
                        {
                            "-r",
                            "${workspaceFolder}/efcpt-config.json",
                        },
                        presentation = new Presentation
                        {
                            reveal = "never",
                        },
                        group = "none",
                        problemMatcher = "$msCompile",
                    },
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
                        presentation = new Presentation
                        {
                            reveal = "always",
                        },
                        group = "none",
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
                            "ErikEJ.EFCorePowerTools.Cli",
                            "--version",
                            $"{version}.0.*-*",
                        },
                        presentation = new Presentation
                        {
                            reveal = "always",
                        },
                        group = "none",
                        problemMatcher = "$msCompile",
                    },
                },
                inputs = new System.Collections.Generic.List<InputItem>
                {
                    new InputItem
                    {
                        type = "promptString",
                        id = "connection",
                        description = "Enter the ADO.NET connection string to use",
                        @default = redactedConnectionString,
                    },
                },
            };

            File.WriteAllText(path, JsonSerializer.Serialize(vsCodeTask, WriteOptions), Encoding.UTF8);
        }
    }
}