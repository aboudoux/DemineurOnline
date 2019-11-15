using System.Collections.Generic;

namespace Demineur.HighScores
{
	public interface IHighScoreRepository
	{
		void AddBesPlayer(string name, int level);
		bool IsHighScore(int level);
		IReadOnlyList<Player> GetBestPlayers();
	}
}