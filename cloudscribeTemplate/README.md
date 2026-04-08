# cloudscribe Project Template

A project template for rapidly creating new ASP.NET Core web applications using the [cloudscribe](https://www.cloudscribe.com) component libraries.

This template provides a complete foundation with:

- **ASP.NET Core Identity** with cloudscribe multi-tenant enhancements
- **Multi-tenancy** support (folder-based, host-based, or single tenant)
- **SimpleContent CMS** for pages and/or blog functionality
- **Bootstrap 5** theming with 20+ built-in themes
- **Modular architecture** for easy customization and scalability
- **Localization** and internationalization support
- **Dynamic authorization policies** for flexible security
- Optional add-ons: Forms, Paywall, Newsletter, Comments, Forum

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022+ (or VS Code with C# Dev Kit)
- Database (SQL Server, PostgreSQL, MySQL, SQLite, or NoDb file system)

### Installation via NuGet

Install the template globally using the .NET CLI:

```bash
dotnet new install cloudscribe.templates
```

### Creating a New Project

**Using .NET CLI:**

```bash
dotnet new cloudscribe --name MyNewApp --DataStorage MSSQL
```

**Using Visual Studio:**

Use the **New Project** dialog and search for "cloudscribe Project Template".

## Template Options

### Core Configuration

- **`--DataStorage`** (required): Choose your data storage provider
  - `NoDb` - File system storage (no database required)
  - `MSSQL` - Microsoft SQL Server (Entity Framework Core)
  - `pgsql` - PostgreSQL (Entity Framework Core)
  - `MySql` - MySQL (Entity Framework Core) ⚠️ Not yet available for .NET 10
  - `SQLite` - SQLite (Entity Framework Core)
  - `AllStorage` - Include all providers (for module development)

- **`--MultiTenantMode`**: Multi-tenancy configuration
  - `FolderName` (default) - Root tenant + folder-based tenants (`/site1/`, `/site2/`)
  - `HostName` - Host-based multi-tenancy (requires DNS configuration)
  - `None` - Single tenant installation

- **`--SimpleContentConfig`**: Content management configuration
  - `a` (default) - Pages and Blog with Pages as default route
  - `b` - Pages and Blog with Home Controller as default route
  - `c` - Blog ONLY with Blog as default route
  - `d` - Blog ONLY with Home Controller as default route
  - `z` - No SimpleContent (excluded)

### Optional Features

- **`--ContactForm`** - Include SimpleContactForm module
- **`--KvpCustomRegistration`** - Include key/value pair custom registration fields
- **`--IdentityServer`** - Include IdentityServer4 integration for OAuth/OpenID Connect
- **`--QueryTool`** - Include admin database query tool (not available with NoDb)
- **`--React`** - Include React/TypeScript integration
- **`--Logging`** - Include cloudscribe logging and log viewer UI (default: true)
- **`--FormBuilder`** - Include Forms and Surveys add-on
- **`--Paywall`** - Include Membership Paywall add-on (not available with NoDb/SQLite)
- **`--Newsletter`** - Include Newsletter/Email List add-on (not available with NoDb/SQLite)
- **`--CommentSystem`** - Include TalkAbout comment system add-on
- **`--Forum`** - Include TalkAbout forum add-on
- **`--DynamicPolicy`** - Include dynamic authorization policies (default: true)

### Example Commands

```bash
# Basic project with SQL Server
dotnet new cloudscribe -n MyApp --DataStorage MSSQL

# Multi-tenant blog with PostgreSQL
dotnet new cloudscribe -n MyBlog --DataStorage pgsql --MultiTenantMode FolderName --SimpleContentConfig c

# Full-featured application with IdentityServer and contact form
dotnet new cloudscribe -n MyPortal --DataStorage MSSQL --IdentityServer true --ContactForm true --DynamicPolicy true
```  

## Resources

- [cloudscribe Documentation](https://www.cloudscribe.com)  
- [GitHub Repository](https://github.com/cloudscribe/cloudscribe.templates)  
- [NuGet Package](https://www.nuget.org/packages/cloudscribe.templates)  
- [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=joeaudette.cloudscribeProjectTemplate)  

## License

This project is licensed under the [Apache 2.0 License](https://opensource.org/licenses/Apache-2.0).

---
