# EFCore.IncludeByExpression
[![Tests](https://github.com/Hau-Hau/EFCore.IncludeByExpression/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/Hau-Hau/EFCore.IncludeByExpression/actions/workflows/test.yml)
[![Coverage Status](https://coveralls.io/repos/github/Hau-Hau/EFCore.IncludeByExpression/badge.svg)](https://coveralls.io/github/Hau-Hau/EFCore.IncludeByExpression)


This repository provides a library that allows to build expressions of only `.Include` and `.ThenInclude` methods.

### Getting started

#### Install from [NuGet](https://www.nuget.org/packages/EFCore.IncludeByExpression)

```powershell
Install-Package EFCore.IncludeByExpression  
```

#### Abstractions can be installed from [NuGet](https://www.nuget.org/packages/EFCore.IncludeByExpression.Abstractions)

```powershell
Install-Package EFCore.IncludeByExpression.Abstractions
```

### Example Usage
```csharp
using EFCore.IncludeByExpression;
using EFCore.IncludeByExpression.Abstractions;

static void Main(string[] args)
{
  var nodes = GetNodes(x => x.Include(y => y.Childs).ThenInclude(y => y.Parent));
}

static IEnumerable<Node> GetNodes(NavigationPropertyPath<Node>? navigationPropertyPath = null)
{
    return DbContext.Nodes.IncludeByExpression(navigationPropertyPath).ToList();
}
```
