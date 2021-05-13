using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using RevEng.Shared;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace RevEng.Core.Tables
{
    /// <summary>
    ///     This is an extension of internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. The base CSharpModelGenerator may be changed or removed without 
    ///     notice in any release. Updating Entity Framework Core version in project may result application failures.
    ///     
    ///     Base class implementation: https://github.com/dotnet/efcore/blob/main/src/EFCore.Design/Scaffolding/Internal/CSharpModelGenerator.cs
    /// </summary>
    public class CSharpModelGeneratorExtend : CSharpModelGenerator
    {
        /// <summary>
        ///     This is an extension of internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. The base CSharpModelGenerator may be changed or removed without 
        ///     notice in any release. Updating Entity Framework Core version in project may result application failures.
        /// </summary>
        private ReverseEngineerCommandOptions ReverseEngineerCommandOptions { get; set; }

        /// <summary>
        ///     This is an extension of internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. The base CSharpModelGenerator may be changed or removed without 
        ///     notice in any release. Updating Entity Framework Core version in project may result application failures.
        /// </summary>
        public CSharpModelGeneratorExtend(
            ModelCodeGeneratorDependencies dependencies,
            ICSharpDbContextGenerator cSharpDbContextGenerator,
            ICSharpEntityTypeGenerator cSharpEntityTypeGenerator,
            ReverseEngineerCommandOptions reverseEngineerCommandOptions)
            : base(dependencies, cSharpDbContextGenerator, cSharpEntityTypeGenerator)
        {
            ReverseEngineerCommandOptions = reverseEngineerCommandOptions;
        }

        private const string FileExtension = ".cs";

        /// <summary>
        ///     This is an extension of internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. The base CSharpModelGenerator may be changed or removed without 
        ///     notice in any release. Updating Entity Framework Core version in project may result application failures.
        /// </summary>
        public override ScaffoldedModel GenerateModel(
            [NotNull] IModel model,
            [NotNull] ModelCodeGenerationOptions options)
        {
            if (options.ContextName == null)
            {
                throw new ArgumentException(
                    CoreStrings.ArgumentPropertyNull(nameof(options.ContextName), nameof(options)), nameof(options));
            }

            if (options.ConnectionString == null)
            {
                throw new ArgumentException(
                    CoreStrings.ArgumentPropertyNull(nameof(options.ConnectionString), nameof(options)), nameof(options));
            }

            // TODO: Honor options.Nullable (issue #15520)
            var generatedCode = CSharpDbContextGenerator.WriteCode(
                model,
                options.ContextName,
                options.ConnectionString,
                options.ContextNamespace,
                options.ModelNamespace,
                options.UseDataAnnotations,
                options.SuppressConnectionStringWarning
#if CORE50 || CORE60
                ,options.SuppressOnConfiguring
#endif
                );
            
            // output DbContext .cs file
            var dbContextFileName = options.ContextName + FileExtension;
            var resultingFiles = new ScaffoldedModel
            {
                ContextFile = new ScaffoldedFile
                {
                    Path = options.ContextDir != null
                        ? Path.Combine(options.ContextDir, dbContextFileName)
                        : dbContextFileName,
                    Code = generatedCode
                }
            };

            foreach (var entityType in model.GetEntityTypes())
            {
                // TODO: Honor options.Nullable (issue #15520)
                generatedCode = CSharpEntityTypeGenerator.WriteCode(entityType, options.ModelNamespace, options.UseDataAnnotations);

                // output EntityType poco .cs file
                var entityTypeFileName = entityType.Name + FileExtension;
                resultingFiles.AdditionalFiles.Add(
                    new ScaffoldedFile 
                    {
                        Path = ReverseEngineerCommandOptions.UseSchemaFolders
                            ? Path.Combine(entityType.FindAnnotation("Relational:Schema")?.Value.ToString() ??
                                           entityType.FindAnnotation("Relational:ViewSchema")?.Value.ToString() ??
                                           "dbo", entityTypeFileName)
                            : entityTypeFileName,
                        Code = generatedCode 
                    });
            }

            return resultingFiles;
        }
    }
}
