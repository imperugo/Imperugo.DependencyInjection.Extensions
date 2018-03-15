namespace Sample.Host.Services
{
	public class Service1 : IService1
	{
		public string SaySomething()
		{
			return $"This message comes from the service {GetType().Name} 'hosted' into Assembly {this.GetType().Assembly.FullName}";
		}
	}
}