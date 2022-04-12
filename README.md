# EFCore.QueryIncludeSchema

This repository provides a library that simplify exposing .Include and .ThenInclude methods.

### Example Usage
```csharp
using EFCore.QueryIncludeSchema.Interfaces;
...
static IEnumerable<Node> GetNodes(IncludePropertyPath<Node>? includePropertyPath = null)
{
    return QueryIncludeSchema
      .For(DbContext.Node)
      .Execute(includePropertyPath)
      .ToList();
}

static void Main(string[] args)
{
  var nodes = GetNodes(x => x 
    .Include(y => y.Parent)
    .ThenInclude(y => y.Childs));
}
```