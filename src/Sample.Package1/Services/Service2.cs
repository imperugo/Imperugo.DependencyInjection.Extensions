namespace Sample.Package1.Services
{
	public class Service2 : IService2
	{
		public string SaySomething()
		{
			return $"This message comes from the service {GetType().Name} 'hosted' into Assembly {this.GetType().Assembly.FullName}";
		}
	}
}