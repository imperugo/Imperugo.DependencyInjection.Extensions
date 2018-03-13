using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Imperugo.DependencyInjection.Extensions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Imperugo.DependencyInjection.Extensions.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddPackageServices(this IServiceCollection services,  IEnumerable<Assembly> assembliesToScan = null)
		{
			var container = services.BuildServiceProvider();

			var logger = container.GetRequiredService<ILoggerFactory>()
				.CreateLogger(nameof(ServiceCollectionExtensions));

			if (assembliesToScan == null)
			{
				assembliesToScan = ScanOnlyTechnogymAssemblies();
			}

			var installers = assembliesToScan
				.SelectMany(s => s.GetTypes())
				.Where(p => typeof(IServiceInstaller).IsAssignableFrom(p) && p.IsClass)
				.ToList();

			foreach (var installer in installers)
			{
				try
				{
					var d = (IServiceInstaller)Activator.CreateInstance(installer);

					d.AddServices(services);
					logger.LogInformation("Added services for {0}", d.GetType().Assembly.GetName().Name);

				}
				catch (Exception e)
				{
					logger.LogError(e, e.Message);
					throw;
				}
			}
		}

		private static IEnumerable<Assembly> ScanOnlyTechnogymAssemblies()
		{
			Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			// Prevent to scan useless assembly and it makes NSB Startup faster
			string dirname = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var directory = new DirectoryInfo(dirname);
			var files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
			var assemblies = new List<Assembly>();
			foreach (var file in files)
			{
				var assemblyName = Path.GetFileNameWithoutExtension(file.Name);

				Assembly alreadyLoadedAssembly = loadedAssemblies.FirstOrDefault(x => x.GetName().Name == assemblyName);
				if (alreadyLoadedAssembly != null)
				{
					assemblies.Add(alreadyLoadedAssembly);
					continue;
				}

				var asbly = Assembly.LoadFrom(file.FullName);
				assemblies.Add(asbly);
			}

			return assemblies;
		}

	}
}
