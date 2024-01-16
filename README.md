# EFCore.NavigationPropertyPathSchema
[![tests](https://github.com/Hau-Hau/EFCore.NavigationPropertyPathSchema/actions/workflows/tests.yml/badge.svg)](https://github.com/Hau-Hau/EFCore.NavigationPropertyPathSchema/actions/workflows/tests.yml)
[![Coverage Status](https://coveralls.io/repos/github/Hau-Hau/EFCore.NavigationPropertyPathSchema/badge.svg)](https://coveralls.io/github/Hau-Hau/EFCore.NavigationPropertyPathSchema)


This repository provides a library that allows to expose only .Include and .ThenInclude methods.

### Getting started

#### Install from [NuGet](https://www.nuget.org/packages/EFCore.NavigationPropertyPathSchema):

```powershell
Install-Package EFCore.NavigationPropertyPathSchema  
```

### Example Usage
```csharp
using EFCore.NavigationPropertyPathSchema;
...
static IEnumerable<Node> GetNodes(IncludePropertyPath<Node>? includePropertyPath = null)
{
    return NavigationPropertyPathSchema
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
