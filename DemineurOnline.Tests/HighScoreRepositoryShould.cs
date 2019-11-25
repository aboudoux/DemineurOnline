using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demineur.HighScores;
using FluentAssertions;
using Xunit;

namespace DemineurOnline.Tests
{
	public class HighScoreRepositoryShould
	{
		readonly IClock _clock = new Clock(new DateTime(2019,11,15, 13,0,0));

		[Fact]
		public void StoreAPlayer()
		{
			using var tempFile = TestTempFile.New;
			var repository = new HighScoreRepository(tempFile.FilePath, _clock);
			repository.AddBesPlayer("Aurelien", 1);
			
			File.Exists(tempFile.FilePath).Should().BeTrue();
			var lines = File.ReadAllLines(tempFile.FilePath);
			lines.Should().HaveCount(1);
			lines.First().Should().Be("\"Aurelien\";1;\"15/11/2019 13:00:00\"");
		}

		[Fact]
		public void RetrieveBestPlayerOrderedByLevel()
		{
			using var tempFile = TestTempFile.New;
			var repository = new HighScoreRepository(tempFile.FilePath, _clock);
			repository.AddBesPlayer("Name1", 1);
			repository.AddBesPlayer("Name2", 2);
			repository.AddBesPlayer("Name3", 3);
			repository.AddBesPlayer("Name4", 4);

			var players = repository.GetBestPlayers();

			players.Should().NotBeEmpty();

			players.Should().BeEquivalentTo(new List<Player>()
			{
				new Player(1,"Name4", 4, _clock.Now),
				new Player(2,"Name3", 3, _clock.Now),
				new Player(3,"Name2", 2, _clock.Now),
				new Player(4,"Name1", 1, _clock.Now),
			}, a=>a.WithStrictOrdering() );
		}


		[Fact]
		public void RetrieveBestPlayerOrderedByLevelAndDate()
		{
			var clock = new FakeClock(new DateTime(2019,11,18, 13,0,0));
			using var tempFile = TestTempFile.New;
			var repository = new HighScoreRepository(tempFile.FilePath, clock);

			repository.AddBesPlayer("Name1", 1);
			clock.IncrementDay();
			repository.AddBesPlayer("Name2", 1);
			clock.IncrementDay();
			repository.AddBesPlayer("Name3", 2);
			clock.IncrementDay();
			repository.AddBesPlayer("Name4", 2);

			var players = repository.GetBestPlayers();

			players.Should().NotBeEmpty();

			players.Should().BeEquivalentTo(new List<Player>()
			{
				new Player(1,"Name3", 2, _clock.Now),
				new Player(2,"Name4", 2, _clock.Now),
				new Player(3,"Name1", 1, _clock.Now),
				new Player(4,"Name2", 1, _clock.Now),
			}, a => a.WithStrictOrdering().Excluding(b=>b.Date));
		}

		[Fact]
		public void ReturnEmptyListIfFileNotExists()
		{
			var repository = new HighScoreRepository("c:\\test\\nothing.csv", new Clock());
			var players = repository.GetBestPlayers();
			players.Should().BeEmpty();

		}

		[Theory]
		[InlineData(9,9,true)]
		[InlineData(10,9,false)]
		[InlineData(10,10,false)]
		[InlineData(10,11,true)]
		public void TellYouIfYourInHightScore(int count, int level, bool expected)
		{
			using var tempFile = TestTempFile.New;
			var repository = new HighScoreRepository(tempFile.FilePath, _clock);

			for(int i = 0; i<count; i++)
				repository.AddBesPlayer("Player", 10);

			repository.IsHighScore(level).Should().Be(expected);
		}
	}
}