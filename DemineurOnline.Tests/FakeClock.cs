using System;
using Demineur.HighScores;

namespace DemineurOnline.Tests
{
	public class FakeClock : IClock
	{
		private readonly DateTime _start;

		public FakeClock(DateTime start)
		{
			_start = start;
		}

		public void IncrementDay() => _start.AddDays(1);

		public DateTime Now => _start;
	}
}