using System;
using System.Collections.Generic;
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
		public static void AddPackageServices(this IServiceCollection services, 
																	IEnumerable<Assembly> assembliesToScan)
		{
			AddPackageServices2(services, null, false, null, assembliesToScan);
		}

		public static void AddPackageServices(this IServiceCollection services,
			Func<string, bool> condition = null,
			bool onlyLoaded = false,
			string[] folderToReadAssemblies = null)
		{
			AddPackageServices2(services, condition, onlyLoaded, folderToReadAssemblies, null);
		}

		private static void AddPackageServices2(this IServiceCollection services, 
																Func<string, bool> condition = null , 
																bool onlyLoaded = false, 
																string[] folderToReadAssemblies = null, 
																IEnumerable<Assembly> assembliesToScan = null)
		{
			var container = services.BuildServiceProvider();

			var logger = container.GetRequiredService<ILoggerFactory>()
				.CreateLogger(nameof(ServiceCollectionExtensions));

			if (assembliesToScan == null)
			{
				assembliesToScan = GetAssembliesToScan(condition, onlyLoaded, folderToReadAssemblies);
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

		private static IEnumerable<Assembly> GetAssembliesToScan(Func<string, bool> condition, 
																								bool onlyLoaded, 
																								string[] folderToReadAssemblies)
		{
			if (condition == null)
			{
				condition = (x) => true;
			}

			if (folderToReadAssemblies == null)
			{
				folderToReadAssemblies = new[] {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)};
			}

			Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var assembliesToScan = new List<Assembly>(loadedAssemblies);

			if (!onlyLoaded)
			{
				var notLoaded = Assembly.GetEntryAssembly().GetReferencedAssemblies();

				foreach (var assemblyName in notLoaded)
				{
					if (condition.Invoke(assemblyName.FullName))
					{
						var loaded = Assembly.Load(assemblyName);
						assembliesToScan.Add(loaded);
					}
				}
			}

			foreach (var folderToReadAssembly in folderToReadAssemblies)
			{
				var directory = new DirectoryInfo(folderToReadAssembly);
				var files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
			
				foreach (var file in files)
				{
					var assemblyName = Path.GetFileNameWithoutExtension(file.Name);

					if (condition.Invoke(assemblyName))
					{
						bool alreadyLoaded =  assembliesToScan.Any(x => x.GetName().FullName == assemblyName);

						if (!alreadyLoaded)
						{
							var loadedAssembly = Assembly.LoadFrom(file.FullName);
							assembliesToScan.Add(loadedAssembly);
						}
					}
				}
			}

			return assembliesToScan;
		}
	}
}
