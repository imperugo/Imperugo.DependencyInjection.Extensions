using Imperugo.DependencyInjection.Extensions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Sample.Package1.Services;

namespace Sample.Package1
{
    internal class ServiceInstaller : IServiceInstaller
    {
	    public IServiceCollection AddServices(IServiceCollection services)
	    {
		    services.AddSingleton<IService2, Service2>();

		    return services;
	    }
    }
}
