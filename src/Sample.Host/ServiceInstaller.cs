using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imperugo.DependencyInjection.Extensions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Sample.Host.Services;

namespace Sample.Host
{
    internal class ServiceInstaller : IServiceInstaller
    {
	    public IServiceCollection AddServices(IServiceCollection services)
	    {
		    services.AddSingleton<IService1, Service1>();

		    return services;
	    }
    }
}
