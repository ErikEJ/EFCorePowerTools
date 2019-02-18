﻿using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using VSLangProj;

namespace EFCorePowerTools.Extensions
{
    using Shared.Enums;

    internal static class ProjectExtensions
    {
        public const int SOk = 0;

        public static bool TryBuild(this Project project)
        {
            var dte = project.DTE;
            var configuration = dte.Solution.SolutionBuild.ActiveConfiguration.Name;

            dte.Solution.SolutionBuild.BuildProject(configuration, project.UniqueName, true);

            return dte.Solution.SolutionBuild.LastBuildInfo == 0;
        }

        public static string GetOutPutAssemblyPath(this Project project)
        {
            var assemblyName = project.Properties.Item("AssemblyName").Value.ToString();

            var assemblyNameExe = assemblyName + ".exe";
            var assemblyNameDll = assemblyName + ".dll";

            var outputPath = GetOutputPath(project);

            if (File.Exists(Path.Combine(outputPath, assemblyNameExe)))
            {
                return Path.Combine(outputPath, assemblyNameExe);
            }

            if (File.Exists(Path.Combine(outputPath, assemblyNameDll)))
            {
                return Path.Combine(outputPath, assemblyNameDll);
            }

            return null;
        }

        public static Tuple<bool, string> ContainsEfCoreReference(this Project project, DatabaseType dbType)
        {
            var providerPackage = "Microsoft.EntityFrameworkCore.SqlServer";
            if (dbType == DatabaseType.SQLCE40)
            {
                providerPackage = "EntityFrameworkCore.SqlServerCompact40";
            }
            if (dbType == DatabaseType.SQLite)
            {
                providerPackage = "Microsoft.EntityFrameworkCore.Sqlite";
            }
            if (dbType == DatabaseType.Npgsql)
            {
                providerPackage = "Npgsql.EntityFrameworkCore.PostgreSQL";
            }
            if (dbType == DatabaseType.Mysql)
            {
                providerPackage = "Pomelo.EntityFrameworkCore.MySql";
            }

            var vsProject = project.Object as VSProject;
            if (vsProject == null) return new Tuple<bool, string>(false, providerPackage);
            for (var i = 1; i < vsProject.References.Count + 1; i++)
            {
                if (vsProject.References.Item(i).Name.Equals(providerPackage))
                {
                    return new Tuple<bool, string>(true, providerPackage);
                }
            }
            return new Tuple<bool, string>(false, providerPackage);
        }

        public static bool IsNetCore(this Project project)
        {
            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.");
        }

        public static bool IsNetCore21(this Project project)
        {
            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.1");
        }

        public static bool IsNetCore22(this Project project)
        {
            return project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETCoreApp,Version=v2.2");
        }

        private static string GetOutputPath(Project project)
        {
            var configManager = project.ConfigurationManager;
            if (configManager == null) return null;

            var activeConfig = configManager.ActiveConfiguration;
            var outputPath = activeConfig.Properties.Item("OutputPath").Value.ToString();
            var fullName = project.FullName;

            var absoluteOutputPath = ReverseEngineer20.PathHelper.GetAbsPath(outputPath, fullName);

            return absoluteOutputPath;
        }

        public static List<string> GenerateFiles(this Project project, List<Tuple<string, string>> result, string extension)
        {
            var list = new List<string>();

            foreach (var item in result)
            {
                var filePath = Path.Combine(Path.GetTempPath(),
                    item.Item1 + extension);

                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                }
                File.WriteAllText(filePath, item.Item2);
                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                list.Add(filePath);
            }
            return list;
        }
    }
}