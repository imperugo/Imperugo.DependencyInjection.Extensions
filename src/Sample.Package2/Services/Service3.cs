namespace Sample.Package2.Services
{
	public class Service3 : IService3
	{
		public string SaySomething()
		{
			return $"This message comes from the service {GetType().Name} 'hosted' into Assembly {this.GetType().Assembly.FullName}";
		}
	}
}