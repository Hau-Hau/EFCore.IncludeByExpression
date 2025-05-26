using System.Runtime.CompilerServices;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal static class Unsafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T As<T>(object o)
            where T : class
        {
            return (T)o;
        }
    }
}
