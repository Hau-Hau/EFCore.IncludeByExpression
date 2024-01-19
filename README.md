# EFCore.IncludeByExpression
[![tests](https://github.com/Hau-Hau/EFCore.IncludeByExpression/actions/workflows/tests.yml/badge.svg)](https://github.com/Hau-Hau/EFCore.IncludeByExpression/actions/workflows/tests.yml)
[![Coverage Status](https://coveralls.io/repos/github/Hau-Hau/EFCore.IncludeByExpression/badge.svg)](https://coveralls.io/github/Hau-Hau/EFCore.IncludeByExpression)


This repository provides a library that allows to build expressions of only `.Include` and `.ThenInclude` methods.

### Getting started

#### Install from [NuGet](https://www.nuget.org/packages/EFCore.IncludeByExpression)

```powershell
Install-Package EFCore.IncludeByExpression  
```

### Example Usage
```csharp
using EFCore.IncludeByExpression;

static void Main(string[] args)
{
  var nodes = GetNodes(x => x.Include(y => y.Childs).ThenInclude(y => y.Parent));
}

static IEnumerable<Node> GetNodes(NavigationPropertyPath<Node>? navigationPropertyPath = null)
{
    return DbContext.Nodes.IncludeByExpression(navigationPropertyPath).ToList();
}
```
