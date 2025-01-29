namespace Ordering.Infrastructure.Exceptions
{
	public class OrderNotFoundException : ApplicationException
	{
		public OrderNotFoundException(string name, Object key) : base($"La entidad {name} - {key} no se encontró.") { }
	}
}