namespace RevEng.Core.Abstractions
{
    public class ModuleScaffolderOptions
    {
        public virtual string ContextName { get; set; }
        public virtual string ContextDir { get; set; }
        public virtual string ContextNamespace { get; set; }
        public virtual string ModelNamespace { get; set; }
        public virtual bool NullableReferences { get; set; }
        public virtual bool ProceduresReturnList { get; set; }
    }
}
