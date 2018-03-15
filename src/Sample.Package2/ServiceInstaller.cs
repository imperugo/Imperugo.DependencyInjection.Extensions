using Imperugo.DependencyInjection.Extensions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Sample.Package2.Services;

namespace Sample.Package2
{
    internal class ServiceInstaller : IServiceInstaller
    {
	    public IServiceCollection AddServices(IServiceCollection services)
	    {
		    services.AddSingleton<IService3, Service3>();

		    return services;
	    }
    }
}
