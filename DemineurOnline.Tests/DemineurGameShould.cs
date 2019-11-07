using FluentAssertions;
using System;
using Xunit;

namespace DemineurOnline.Tests
{
	public class DemineurGameShould
	{
		[Fact]
		public void CreateNewGameBoard()
		{
			var game =  GameBoard.Create(5, 10);
			game.Should().NotBeNull();
		}

		[Fact]
		public void ThowErrorIfDemandMoreBombsThenZone()
		{
			Action action = () => GameBoard.Create(5, 25);
			action.Should().Throw<Exception>();
		}

		[Fact]
		public void ShowGameState()
		{
			var game = GameBoard.Create(5, 10);
			game.Should().HaveCount(25);
			game.Should().AllBeAssignableTo<UndiscoveredCell>();
		}
	}
}
