using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demineur
{
	public class Game
	{
		public int Level { get; private set; }

		private const int StartZoneSize = 3;
		private const int StartRatio= 10;

		public MineField Start()
		{
			Level = 1;
			return MineField.Create(StartZoneSize, Level);
		}

		public MineField Next()
		{
			Level++;
			return MineField.Create(GetZoneSize(), Level);

			int GetZoneSize()
			{
				var percent = (Level * 100) / (StartRatio + (Level/3));
				return (int)Math.Sqrt(percent);
			}
		}
	}
}