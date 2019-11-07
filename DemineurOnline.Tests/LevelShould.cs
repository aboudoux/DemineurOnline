using Demineur;
using FluentAssertions;
using Xunit;

namespace DemineurOnline.Tests
{
	public class LevelShould
	{
		[Fact]
		public void CreateFirstLevel()
		{
			var game = new Game();
			MineField board = game.Start();
			board.ZoneSize.Should().Be(3);
			board.BombCount.Should().Be(1);

		}

		[Theory]
		[InlineData(2, 4,2)]
		[InlineData(3, 5,3)]
		[InlineData(4, 6,4)]
		[InlineData(5, 6,5)]
		[InlineData(6, 7,6)]
		[InlineData(7, 7,7)]
		[InlineData(8, 8,8)]
		[InlineData(9, 8,9)]
		[InlineData(10, 8,10)]
		public void CreateNextLevel(int level, int expectedSize, int expectedBombs)
		{
			var game = new Game();
			var board = game.Start();

			for(int i = 1; i < level; i++)
				board = game.Next();

			board.BombCount.Should().Be(expectedBombs);
			board.ZoneSize.Should().Be(expectedSize);
			game.Level.Should().Be(level);
		}
	}
}