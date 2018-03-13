using Microsoft.Extensions.DependencyInjection;

namespace Imperugo.DependencyInjection.Extensions.Abstractions
{
	public interface IServiceInstaller
	{
		IServiceCollection AddServices(IServiceCollection services);
	}
}