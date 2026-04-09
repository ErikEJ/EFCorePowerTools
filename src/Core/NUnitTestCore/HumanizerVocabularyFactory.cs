using System;
using System.Reflection;
using Humanizer.Inflections;

namespace UnitTests
{
    internal static class HumanizerVocabularyFactory
    {
        private static readonly MethodInfo BuildDefaultMethod =
            typeof(Vocabularies).GetMethod("BuildDefault", BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find Humanizer vocabulary builder.");

        public static Vocabulary CreateDefault()
            => (Vocabulary)BuildDefaultMethod.Invoke(null, null)!;
    }
}
