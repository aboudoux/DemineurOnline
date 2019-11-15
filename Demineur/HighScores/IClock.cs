using System;

namespace Demineur.HighScores
{
	public interface IClock
	{
		DateTime Now { get; }
	}

	public class Clock : IClock
	{
		private readonly DateTime? _now;

		public Clock(DateTime? now = null)
		{
			_now = now;
		}

		public DateTime Now => _now.HasValue ? _now.Value : DateTime.Now;
	}
}