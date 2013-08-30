
using System;
namespace Discovery.Slp.Services
{
	/// <summary>
	/// Interface for service location provisers
	/// </summary>
	public interface IServiceProvider
	{
		/// <summary>
		/// Get an instance of a type
		/// </summary>
		/// <typeparam name="T">the type to be returned</typeparam>
		/// <param name="arguments">an optional array of arguments</param>
		/// <returns>an instance of T</returns>
		T GetInstance<T>(params object[] arguments);

		object GetInstance(Type type, params object[] arguments);
	}
}
