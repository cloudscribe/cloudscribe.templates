# cloudscribe Project Template

A project template for rapidly creating new ASP.NET Core web applications using the [cloudscribe](https://www.cloudscribe.com) component libraries.

This template includes pre-configured support for:

- ASP.NET Core Identity with cloudscribe enhancements  
- Multi-tenancy  
- Navigation menus  
- Localization  
- Bootstrap-based layout and theming  
- Modular structure for scalability  

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)  
- Visual Studio 2022+ or VS Code  
- Optionally: [NuGet CLI](https://www.nuget.org/downloads)  

### Installation via NuGet

You can install the template globally using the .NET CLI:

```
dotnet new install cloudscribe.templates
```

### Creating a New Project

After installation, create a new project with:

```
dotnet new cloudscribe --name MyNewApp
```

Or use the Visual Studio **New Project** dialog and search for "cloudscribe Project Template".

## Template Options

When creating a project, the template will prompt for:

- **Project Name**: Your application name  
- **Database Provider**: SQL Server, PostgreSQL, or SQLite  
- **Multi-Tenant Support**: Yes or No  
- **UI Theme**: Default Bootstrap or Minimal  

## Resources

- [cloudscribe Documentation](https://www.cloudscribe.com)  
- [GitHub Repository](https://github.com/cloudscribe/cloudscribe.templates)  
- [NuGet Package](https://www.nuget.org/packages/cloudscribe.templates)  
- [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=joeaudette.cloudscribeProjectTemplate)  

## License

This project is licensed under the [Apache 2.0 License](https://opensource.org/licenses/Apache-2.0).

---
