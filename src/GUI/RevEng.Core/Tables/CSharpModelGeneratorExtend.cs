using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using RevEng.Shared;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal
{
    public class CSharpModelGeneratorExtend : CSharpModelGenerator
    {
        private bool UseSchemaFolders { get; set; }

        public CSharpModelGeneratorExtend(
            ModelCodeGeneratorDependencies dependencies,
            ICSharpDbContextGenerator cSharpDbContextGenerator,
            ICSharpEntityTypeGenerator cSharpEntityTypeGenerator,
            bool useSchemaFolders)
            : base(dependencies, cSharpDbContextGenerator, cSharpEntityTypeGenerator)
        {
            UseSchemaFolders = useSchemaFolders;
        }

        private const string FileExtension = ".cs";

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
                var entityTypeSchema = entityType.GetSchema();
#if CORE50 || CORE60
                if (entityType.GetViewName() != null)
                {
                    entityTypeSchema = entityType.GetViewSchema();
                }
#endif

                resultingFiles.AdditionalFiles.Add(
                    new ScaffoldedFile 
                    {
                        Path = UseSchemaFolders && !string.IsNullOrEmpty(entityTypeSchema)
                            ? Path.Combine(entityTypeSchema, entityTypeFileName)
                            : entityTypeFileName,
                        Code = generatedCode 
                    });
            }

            return resultingFiles;
        }
    }
}
