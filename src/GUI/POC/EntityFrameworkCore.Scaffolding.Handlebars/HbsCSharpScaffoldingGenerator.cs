// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Modifications copyright(C) 2017 Tony Sneed.

using System;
using EntityFrameworkCore.Scaffolding.Handlebars.Helpers;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Scaffolding generator for DbContext and entity type classes using Handlebars templates.
    /// </summary>
    public class HbsCSharpScaffoldingGenerator : ScaffoldingCodeGenerator
    {
        /// <summary>
        /// DbContext template service.
        /// </summary>
        public virtual IDbContextTemplateService DbContextTemplateService { get; }

        /// <summary>
        /// Entity type template service.
        /// </summary>
        public virtual IEntityTypeTemplateService EntityTypeTemplateService { get; }

        /// <summary>
        /// DbContext generator.
        /// </summary>
        public virtual ICSharpDbContextGenerator CSharpDbContextGenerator { get; }

        /// <summary>
        /// Entity type generator.
        /// </summary>
        public virtual ICSharpEntityTypeGenerator CSharpEntityTypeGenerator { get; }

        /// <summary>
        /// Constructor for the Handlebars scaffolding generator.
        /// </summary>
        /// <param name="fileService">Provides files to the template service.</param>
        /// <param name="dbContextTemplateService">Template service for DbContext generator.</param>
        /// <param name="entityTypeTemplateService">Template service for the entity types generator.</param>
        /// <param name="cSharpDbContextGenerator">DbContext generator.</param>
        /// <param name="cSharpEntityTypeGenerator">Entity types generator.</param>
        public HbsCSharpScaffoldingGenerator(
            ITemplateFileService fileService,
            IDbContextTemplateService dbContextTemplateService,
            IEntityTypeTemplateService entityTypeTemplateService,
            ICSharpDbContextGenerator cSharpDbContextGenerator,
            ICSharpEntityTypeGenerator cSharpEntityTypeGenerator)
          : base(fileService)
        {
            DbContextTemplateService = dbContextTemplateService ?? throw new ArgumentNullException(nameof(dbContextTemplateService));
            EntityTypeTemplateService = entityTypeTemplateService ?? throw new ArgumentNullException(nameof(entityTypeTemplateService));
            CSharpDbContextGenerator = cSharpDbContextGenerator ?? throw new ArgumentNullException(nameof(cSharpDbContextGenerator));
            CSharpEntityTypeGenerator = cSharpEntityTypeGenerator ?? throw new ArgumentNullException(nameof(cSharpEntityTypeGenerator));
        }

        /// <summary>
        /// File extension for generated files.
        /// </summary>
        public override string FileExtension => ".cs";

        /// <summary>
        /// Reverse engineer DbContext and entity type files from an existing database.
        /// </summary>
        /// <param name="model">Metadata about the shape of entities, the relationships between them, and how they map to the database.</param>
        /// <param name="outputPath">File path for the generated files.</param>
        /// <param name="namespace">Namespace for generated classes.</param>
        /// <param name="contextName">DbContext name.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="useDataAnnotations">Generate classes using data annotations.</param>
        /// <returns></returns>
        public override ReverseEngineerFiles WriteCode(
            IModel model, 
            string outputPath, 
            string @namespace, 
            string contextName, 
            string connectionString, 
            bool useDataAnnotations)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));
            if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));
            if (contextName == null) throw new ArgumentNullException(nameof(contextName));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            // Register Hbs helpers and partial templates
            DbContextTemplateService.RegisterHelper(Constants.SpacesHelper, HandlebarsHelpers.GetSpacesHelper());
            DbContextTemplateService.RegisterPartialTemplates();
            EntityTypeTemplateService.RegisterPartialTemplates();

            ReverseEngineerFiles reverseEngineerFiles = new ReverseEngineerFiles();

            if (!(CSharpDbContextGenerator is NullCSharpDbContextGenerator))
            {
                string contents1 = CSharpDbContextGenerator.WriteCode(model, @namespace, contextName, connectionString, useDataAnnotations);
                string fileName1 = contextName + FileExtension;
                string str1 = FileService.OutputFile(outputPath, fileName1, contents1);
                reverseEngineerFiles.ContextFile = str1;
            }

            if (!(CSharpEntityTypeGenerator is NullCSharpEntityTypeGenerator))
            {
                foreach (IEntityType entityType in model.GetEntityTypes())
                {
                    string contents2 = CSharpEntityTypeGenerator.WriteCode(entityType, @namespace, useDataAnnotations);
                    string fileName2 = entityType.DisplayName() + FileExtension;
                    string str2 = FileService.OutputFile(outputPath, fileName2, contents2);
                    reverseEngineerFiles.EntityTypeFiles.Add(str2);
                }
            }

            return reverseEngineerFiles;
        }
    }
}
