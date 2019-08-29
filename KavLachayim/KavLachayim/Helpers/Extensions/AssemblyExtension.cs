using System;
using System.Linq;
using System.Reflection;

namespace KavLachayim.Helpers.Extensions
{
    public static class AssemblyExtension
    {
        public static Type GetTypeWithoutNamespace(this Assembly assembly, string typeName)
        {
            var foundType = (from t in assembly.DefinedTypes
                             where t.Name == typeName
                             select t).First().AsType();

            return foundType;
        }
    }
}
