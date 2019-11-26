using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System;
using System.Collections.Generic;

namespace ReverseEngineer20.ReverseEngineer
{
    public class CustomerCSharpUniqueNamer<T> : CSharpUniqueNamer<T>
    {
        private readonly Func<T, string> _nameGetter;
        private readonly Func<T, bool> _hasCustomer;
        private readonly HashSet<string> _usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public CustomerCSharpUniqueNamer([NotNull] Func<T, string> nameGetter,
            [CanBeNull] IEnumerable<string> usedNames,
            [NotNull] ICSharpUtilities cSharpUtilities,
            [CanBeNull] Func<string, string> singularizePluralizer) :
            base(nameGetter, usedNames, cSharpUtilities, singularizePluralizer){}

        public CustomerCSharpUniqueNamer(
            [NotNull] Func<T, bool> hasCustomer,
            [NotNull] Func<T, string> nameGetter,
            [NotNull] ICSharpUtilities cSharpUtilities,
            [CanBeNull] Func<string, string> singularizePluralizer) :
            base(nameGetter, cSharpUtilities, singularizePluralizer)
        {
            _hasCustomer = hasCustomer;
            _nameGetter = nameGetter;
        }

        public override string GetName(T item)
        {
            if (NameCache.ContainsKey(item))
            {
                if (!_hasCustomer(item))
                    return NameCache[item];

                return base.GetName(item);
            }

            if (_hasCustomer(item))
            {
                NameCache[item] = _nameGetter(item);
                return NameCache[item];
            }

            var input = base.GetName(item);
            var name = input;
            var suffix = 1;

            while (_usedNames.Contains(name))
            {
                name = input + suffix++;
            }

            _usedNames.Add(name);
            NameCache[item] = name;

            return NameCache[item];
        }
    }
}
