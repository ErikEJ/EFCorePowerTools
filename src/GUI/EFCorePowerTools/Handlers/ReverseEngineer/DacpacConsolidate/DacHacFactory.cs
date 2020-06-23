namespace GOEddie.Dacpac.References
{
    public class DacHacFactory
    {
        public DacHacXml Build(string path)
        {
            return new DacHacXml(path);
        }
    }
}