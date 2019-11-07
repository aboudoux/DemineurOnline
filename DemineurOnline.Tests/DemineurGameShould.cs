using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Demineur;
using Demineur.Exceptions;
using Xunit;

namespace DemineurOnline.Tests
{
	public class DemineurGameShould
	{
		[Fact]
		public void CreateNewGameBoard()
		{
			var game =  MineField.Create(5, 10);
			game.Should().NotBeNull();
		}

		[Fact]
		public void ThowErrorIfDemandMoreBombsThenZone()
		{
			Action action = () => MineField.Create(5, 25);
			action.Should().Throw<Exception>();
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-10)]
		public void ThrowErrorIfNoBombProvided(int numberOfBombs)
		{
			Action action = () => MineField.Create(5, numberOfBombs);
			action.Should().Throw<NoBombException>();
		}

		[Fact]
		public void ShowGameState()
		{
			var game = MineField.Create(5, 10);
			game.Should().HaveCount(25);
			game.Should().AllBeAssignableTo<UndiscoveredCell>();
		}

		[Fact]
		public void GenerateSomeCellWithHiddenBomb()
		{
			var game = MineField.Create(5, 10);
			game.OfType<UndiscoveredCell>().Count(a => a.HasBomb).Should().Be(10);
		}

		[Fact]
		public void PoseFlagOnUndiscoveredCell()
		{
			var game = MineField.Create(5, 10);
			game.PoseFlag(1, 1);
			game[1, 1].Should().BeAssignableTo<FlaggedCell>();
		}

		[Fact]
		public void UnFlagAFlaggedCell()
		{
			var game = MineField.Create(5, 10);
			game.PoseFlag(1, 1);
			game.PoseFlag(1, 1);
			game[1, 1].Should().BeAssignableTo<UndiscoveredCell>();
		}

		[Fact]
		public void RevealABomb()
		{
			var game = MineField.Create(5, 10);
			var trappedCell = game.OfType<UndiscoveredCell>().First(a => a.HasBomb);
			game.Reveal(trappedCell);

			game.Fail.Should().BeTrue();
		}

		[Fact]
		public void NotRevealFlaggedCell()
		{
			var game = MineField.Create(5, 10);
			var trappedCell = game.OfType<UndiscoveredCell>().First(a => a.HasBomb);
			game.PoseFlag(trappedCell);
			game.Reveal(trappedCell);

			game[trappedCell].Should().BeAssignableTo<FlaggedCell>();
			game.Fail.Should().BeFalse();
		}

		[Fact]
		public void DontPoseMoreFlagsThanBombs()
		{
			var game = MineField.Create(5, 10);
			game.ForEach(cell => game.PoseFlag(cell));

			game.FlaggedCellCount.Should().Be(10);
		}

		[Fact]
		public void WinIfAllFlagArePlacedOnBomb()
		{
			var game = MineField.Create(5, 10);
			game.Where(a=>(a as UndiscoveredCell).HasBomb).ForEach(cell => game.PoseFlag(cell));
			game.Win.Should().BeTrue();
		}

		[Theory]
		[InlineData(-1,0)]
		[InlineData(0,-1)]
		[InlineData(6,1)]
		[InlineData(1,6)]
		[InlineData(5,5)]
		public void DontThrowErrorIfPoseFlagIsOutOfIndex(int row, int columns)
		{
			var game = MineField.Create(5, 10);
			game.PoseFlag(row, columns);
			game.Should().AllBeAssignableTo<UndiscoveredCell>();
		}

		[Theory]
		[InlineData(-1, 0)]
		[InlineData(0, -1)]
		[InlineData(6, 1)]
		[InlineData(1, 6)]
		[InlineData(5, 5)]
		public void DontThrowErrorIfRevealIsOutOfIndex(int row, int columns)
		{
			var game = MineField.Create(5, 10);
			game.Reveal(row, columns);
			game.Should().AllBeAssignableTo<UndiscoveredCell>();
		}


		[Fact]
		public void RevealEmptyCell()
		{
			var game = MineField.Create(5, 10);
			var emptyCell = game.OfType<UndiscoveredCell>().First(a => !a.HasBomb);
			game.Reveal(emptyCell);
			game[emptyCell].Should().BeAssignableTo<EmptyCell>();
		}

		[Fact]
		public void RevealEmptyCellAroundIfOpenEmptyCell()
		{
			var game = MineField.Build(3, new List<ICell>()
			{
				new UndiscoveredCell(0, 0),
				new UndiscoveredCell(0, 1),
				new UndiscoveredCell(0, 2),
				new UndiscoveredCell(1, 0),
				new UndiscoveredCell(1, 1),
				new UndiscoveredCell(1, 2),
				new UndiscoveredCell(2, 0),
				new UndiscoveredCell(2, 1),
				new UndiscoveredCell(2, 2),
			});

			game.Reveal(1,1);
			game.Should().AllBeAssignableTo<EmptyCell>();
		}

		[Fact]
		public void RevealNumberCellAroundIfOpenEmptyCellNearBomb()
		{
			var game = MineField.Build(3, new List<ICell>()
			{
				new UndiscoveredCell(0, 0){ HasBomb = true},
				new UndiscoveredCell(0, 1){ NumberOfBombsAround = 1},
				new UndiscoveredCell(0, 2),
				new UndiscoveredCell(1, 0){ NumberOfBombsAround = 1},
				new UndiscoveredCell(1, 1){ NumberOfBombsAround = 1},
				new UndiscoveredCell(1, 2),
				new UndiscoveredCell(2, 0),
				new UndiscoveredCell(2, 1),
				new UndiscoveredCell(2, 2),
			});

			game.Reveal(1, 0);

			game[0, 0].Should().BeAssignableTo<UndiscoveredCell>();
			game[0, 1].Should().BeAssignableTo<NumberCell>();
			game[0, 2].Should().BeAssignableTo<UndiscoveredCell>();
			game[1, 0].Should().BeAssignableTo<EmptyCell>();
			game[1, 1].Should().BeAssignableTo<NumberCell>();
			game[1, 2].Should().BeAssignableTo<UndiscoveredCell>();
			game[2, 0].Should().BeAssignableTo<EmptyCell>();
			game[2, 1].Should().BeAssignableTo<EmptyCell>();
			game[2, 2].Should().BeAssignableTo<UndiscoveredCell>();
		}
	}
}
