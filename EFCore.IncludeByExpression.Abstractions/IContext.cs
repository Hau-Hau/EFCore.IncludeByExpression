using System.Linq;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal interface IContext
    {
        IQueryable Query { get; set; }
    }
}
