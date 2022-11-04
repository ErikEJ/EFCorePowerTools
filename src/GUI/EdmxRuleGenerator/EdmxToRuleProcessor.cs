using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EdmxRuleGenerator.EdmxModel;
using EdmxRuleGenerator.Extensions;
using RevEng.Common;
using RevEng.Common.Extensions;
using Exception = System.Exception;

namespace EdmxRuleGenerator;

public class EdmxToRuleProcessor
{
  

    public string EdmxFilePath { get; }
    public string ProjectBasePath { get; }
    public List<string> Errors { get; } = new();
    public Dictionary<string, object> RulesGeneratedByFile { get; } = new();
    public EdmxRuleGenerator Generator { get; private set; }

    internal static bool TryParseArgs(string[] args, out string edmxPath, out string projectBasePath)
    {
        edmxPath = null;
        projectBasePath = null;
        if (args.IsNullOrEmpty() || (args.Length == 1 && args[0] == "."))
        {
            // auto inspect current folder for both csproj and edmx
            projectBasePath = Directory.GetCurrentDirectory();
            var projectFiles = Directory.GetFiles(projectBasePath, "*.csproj", SearchOption.TopDirectoryOnly);
            if (projectFiles.Length != 1)
            {
                projectBasePath = null;
                return false;
            }

            var edmxFiles = Directory.GetFiles(projectBasePath, "*.edmx");
            if (edmxFiles.Length != 1)
            {
                return false;
            }

            edmxPath = edmxFiles[0];
            return true;
        }

        edmxPath = args.FirstOrDefault(o => o?.EndsWith(".edmx", StringComparison.OrdinalIgnoreCase) == true);
        if (edmxPath == null)
        {
            // inspect arg paths for edmx
            foreach (var arg in args)
            {
                if (arg.IsNullOrWhiteSpace() || !Directory.Exists(arg))
                {
                    continue;
                }

                var edmxFiles = Directory.GetFiles(arg, "*.edmx", SearchOption.TopDirectoryOnly);
                if (edmxFiles.Length == 0)
                {
                    continue;
                }

                if (edmxFiles.Length > 1)
                {
                    return false;
                }

                edmxPath = edmxFiles[0];
                break;
            }
        }

        if (edmxPath.IsNullOrEmpty() || !File.Exists(edmxPath))
        {
            return false;
        }

        projectBasePath =
            args.FirstOrDefault(o => o?.EndsWith(".edmx", StringComparison.OrdinalIgnoreCase) == false);
        if (projectBasePath.IsNullOrWhiteSpace() || projectBasePath == ".")
        {
            projectBasePath = Directory.GetCurrentDirectory();
        }

        if (!Directory.Exists(projectBasePath))
        {
            if (File.Exists(projectBasePath))
            {
                projectBasePath = new FileInfo(projectBasePath).Directory?.FullName;
                if (projectBasePath == null || !Directory.Exists(projectBasePath))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        var projectFiles2 = Directory.GetFiles(projectBasePath, "*.csproj", SearchOption.TopDirectoryOnly);
        if (projectFiles2.Length == 0)
        {
            projectBasePath = null;
            return false;
        }

        return true;
    }


    public EdmxToRuleProcessor(string edmxFilePath, string projectBasePath)
    {
        EdmxFilePath = edmxFilePath;
        ProjectBasePath = projectBasePath;
    }

    public bool TryProcess()
    {
        try
        {
            Generator = new EdmxRuleGenerator(EdmxParser.Parse(EdmxFilePath));
        }
        catch (Exception ex)
        {
            Errors.Add($"Error parsing EDMX: {ex.Message}");
            return false;
        }

        GenerateAndWrite(() => Generator.GetTableAndPropertyRenameRules(), RuleFiles.RenamingFilename);
        GenerateAndWrite(() => Generator.GetNavigationRenameRules(), RuleFiles.PropertyFilename);
        GenerateAndWrite(() => Generator.GetEnumMappingRules(), RuleFiles.EnumMappingFilename);
        return Errors.Count == 0;
    }

    private void GenerateAndWrite<T>(Func<T> gen, string fileName) where T : class
    {
        try
        {
            var rulesRoot = gen();
            WriteRules(rulesRoot, fileName);
            RulesGeneratedByFile.Add(fileName, rulesRoot);
        }
        catch (Exception ex)
        {
            Errors.Add($"Error generating output for {fileName}: {ex.Message}");
        }
    }

    private void WriteRules<T>(T rules, string filename)
        where T : class
    {
        var path = Path.Combine(ProjectBasePath, filename);
        rules.ToJson<T>(path);
    }
}
