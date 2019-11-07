using System;

namespace Demineur.Exceptions
{
	public class NoBombException : Exception
	{
		public NoBombException() : base("You must provide a number of bombs greater than 0")
		{
		}
	}
}
