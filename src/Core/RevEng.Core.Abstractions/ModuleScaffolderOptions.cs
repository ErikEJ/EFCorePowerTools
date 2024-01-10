[assembly: System.CLSCompliant(true)]

namespace RevEng.Core.Abstractions
{
    public class ModuleScaffolderOptions
    {
        public virtual string ContextName { get; set; }
        public virtual string ContextDir { get; set; }
        public virtual string ContextNamespace { get; set; }
        public virtual string ModelNamespace { get; set; }
        public virtual bool NullableReferences { get; set; }
        public virtual bool UseSchemaFolders { get; set; }
        public virtual bool UseAsyncCalls { get; set; }
        public virtual bool UseSchemaNamespaces { get; set; }
        public virtual bool UseDecimalDataAnnotation { get; set; }
    }
}
