# Imperugo.DependencyInjection.Extensions

**Imperugo.DependencyInjection.Extensions** is a library that allows you to auto register packages into your Microsoft Dependency Injection framework
The library is signed and completely compatible with the **.Net Standard 2.0**


##Install via Nuget 

```
PM> Install-Package SImperugo.DependencyInjection.Extensions
```

## How to use it
It is user easy.
For each package is enough to create a class (internal is fine so no-one can create an instance) that implements `IServiceInstaller`

```csharp
internal class PackageInstaller : IDependencyInstaller
{
	public IServiceCollection AddServices(IServiceCollection services)
	{
		services.AddSingleton<IUserService, UserService>();
		services.AddSingleton<IRoleService, RoleService>();

		return services;
	}
}
```

On your `Startup.cs` class:

```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
	services.AddPackageServices();

	//.....
}
```
If you want to improve your startup time there is a specific overload that allows you to specify only the assemblies to scan:


```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
	var assembliesToScan = AppDomain
			.CurrentDomain
			.GetAssemblies()
			.Where(x => x.GetName().Name.StartsWith("MyNamespace"))
			.ToList();

	services.AddPackageServices(assembliesToScan);

	//.....
}
```

## Contributing
**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide/)
 * [Open an issue](https://github.com/imperugo/StackExchange.Redis.Extensions/issues) if you encounter a bug or have a suggestion for improvements/features


Once you're familiar with Git and GitHub, clone the repository and start contributing.
