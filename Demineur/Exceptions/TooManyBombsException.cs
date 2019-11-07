using System;

namespace Demineur.Exceptions
{
	public class TooManyBombsException : Exception
	{
		public TooManyBombsException() : base("There are too many bombs on the game board")
		{
		}
	}
}