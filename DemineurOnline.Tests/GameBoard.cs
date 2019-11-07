using System.Collections;
using System.Collections.Generic;

namespace DemineurOnline.Tests
{
	public class GameBoard : IEnumerable<ICell>
	{
		private readonly ICell[,] _cells;

		private GameBoard(int zoneSize, int numberOfBombs)
		{
			_cells = new ICell[zoneSize,zoneSize];
			for (var line = 0; line < zoneSize; line++)
			{
				for(var row = 0; row < zoneSize; row++)
					_cells[line, row] = new UndiscoveredCell(line, row);
			}
		}

		public static GameBoard Create(int zoneSize, int numberOfBombs) => new GameBoard(zoneSize, numberOfBombs);

		public IEnumerator<ICell> GetEnumerator()
		{
			foreach (var cell in _cells)
				yield return cell;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public interface ICell
	{
		int Line { get; }
		int Row { get; }
	}

	public class UndiscoveredCell : Cell {
		public UndiscoveredCell(int line, int row) : base(line, row)
		{
		}
	}
	public class EmptyCell : Cell {
		public EmptyCell(int line, int row) : base(line, row)
		{
		}
	}
	public class BombCell : Cell {
		public BombCell(int line, int row) : base(line, row)
		{
		}
	}
	public class FlaggedCell : Cell {
		public FlaggedCell(int line, int row) : base(line, row)
		{
		}
	}
	public class NumberCell : Cell {
		public NumberCell(int line, int row) : base(line, row)
		{
		}
	}

	public abstract class Cell : ICell
	{
		protected Cell(int line, int row)
		{
			Line = line;
			Row = row;
		}
		public int Line { get; }
		public int Row { get; }
	}
}