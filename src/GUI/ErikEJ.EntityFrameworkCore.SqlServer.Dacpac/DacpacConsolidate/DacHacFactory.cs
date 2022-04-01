namespace GOEddie.Dacpac.References
{
    public static class DacHacFactory
    {
        public static DacHacXml Build(string path)
        {
            return new DacHacXml(path);
        }
    }
}
