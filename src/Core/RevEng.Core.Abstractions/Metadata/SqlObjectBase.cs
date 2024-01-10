namespace RevEng.Core.Abstractions.Metadata
{
    public class SqlObjectBase
    {
        public virtual string Name { get; set; }
        public virtual string NewName { get; set; }
        public virtual string Schema { get; set; }
    }
}
