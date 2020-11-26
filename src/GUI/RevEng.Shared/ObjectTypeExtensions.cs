namespace RevEng.Shared
{
    public static class ObjectTypeExtensions
    {
        public static bool HasColumns(this ObjectType input) => input == ObjectType.View || input == ObjectType.Table;
    }
}
