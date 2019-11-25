using System;

namespace Demineur.HighScores
{
	public class Player
	{
		public Player()
		{
			
		}
		public Player(int position, string name, int level, DateTime date)
		{
			Position = position;
			Name = name;
			Level = level;
			Date = date;
		}

		public int Position { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public DateTime Date { get; set; }
	}
}