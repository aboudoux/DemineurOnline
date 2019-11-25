using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FluentCsv.FluentReader;

namespace Demineur.HighScores
{
	public class HighScoreRepository : IHighScoreRepository
	{
		private readonly string _filePath;
		private readonly IClock _clock;
		private readonly  CultureInfo _currentCulture = new CultureInfo("fr-FR");
		private const int BestOfCount = 10;

		public HighScoreRepository(string filePath, IClock clock)
		{
			if(string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
			_filePath = filePath;
			_clock = clock ?? throw new ArgumentNullException(nameof(clock));
		}

		public void AddBesPlayer(string name, int level)
		{
			File.AppendAllText(_filePath, $"\"{name}\";{level};\"{_clock.Now.ToString(_currentCulture)}\"\r\n");
		}

		public bool IsHighScore(int level)
		{
			var bestPlayers = GetBestPlayers().Take(BestOfCount);
			return bestPlayers.Count() < BestOfCount || bestPlayers.Any(a => a.Level < level);
		}

		public IReadOnlyList<Player> GetBestPlayers()
		{
			if(!File.Exists(_filePath))
				return new List<Player>();

			var position = 1;
			var csv = Read.Csv.FromFile(_filePath)
				.With.CultureInfo(_currentCulture.Name)
				.ThatReturns.ArrayOf<Player>()
				.Put.Column(0).Into(a=>a.Name)
				.Put.Column(1).As<int>().Into(a=>a.Level)
				.Put.Column(2).As<DateTime>().Into(a=>a.Date)
				.GetAll();

			return csv.ResultSet.OrderByDescending(a=>a.Level)
				         .ForEach(a => a.Position = position++)
				         .ToList();
		}
	}
}