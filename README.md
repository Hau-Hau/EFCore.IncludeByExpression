# EFCore.QueryIncludeSchema
[![tests](https://github.com/Hau-Hau/EFCore.QueryIncludeSchema/actions/workflows/tests.yml/badge.svg)](https://github.com/Hau-Hau/EFCore.QueryIncludeSchema/actions/workflows/tests.yml)
[![Coverage Status](https://coveralls.io/repos/github/Hau-Hau/EFCore.QueryIncludeSchema/badge.svg)](https://coveralls.io/github/Hau-Hau/EFCore.QueryIncludeSchema)


This repository provides a library that allows to expose only .Include and .ThenInclude methods.

### Getting started

#### Install from [NuGet](https://www.nuget.org/packages/EFCore.QueryIncludeSchema):

```powershell
Install-Package EFCore.QueryIncludeSchema  
```

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
