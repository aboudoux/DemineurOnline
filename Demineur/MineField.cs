using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Demineur.Exceptions;

namespace Demineur
{
	public class MineField : IEnumerable<ICell>
	{
		private readonly ICell[,] _cells;

		public bool Fail => _cells.OfType<BombCell>().Any();
		public int FlaggedCellCount { get; private set; }
		public int BombCount { get; }
		public bool Win => FlaggedCellCount == BombCount && _cells.OfType<FlaggedCell>().All(a => a.HasBomb);

		public int ZoneSize { get; }

		private MineField(int zoneSize, IEnumerable<ICell> cells)
		{
			if (cells == null) throw new ArgumentNullException(nameof(cells));
			if(cells.Count() != zoneSize * zoneSize)
				throw new Exception("Le nombre de cellule doit être le même que la dimension du tableau");

			ZoneSize = zoneSize;
			_cells = new ICell[zoneSize,zoneSize];

			var enumerator = cells.GetEnumerator();

			for (var row = 0; row < zoneSize; row++)
			{
				for (var column = 0; column < zoneSize; column++)
				{
					enumerator.MoveNext();
					_cells[row, column] = enumerator.Current;
				}
			}

		}
		private MineField(int zoneSize, int numberOfBombs)
		{
			if(numberOfBombs <= 0)
				throw new NoBombException();

			if (numberOfBombs >= zoneSize * zoneSize)
				throw new TooManyBombsException();

			BombCount = numberOfBombs;
			ZoneSize = zoneSize;

			_cells = new ICell[zoneSize,zoneSize];

			GenerateGameCells();
			PutBombs();
			ScanNumberOfBombsAroundEmptyCells();

			void GenerateGameCells()
			{
				for (var row = 0; row < zoneSize; row++)
				{
					for (var column = 0; column < zoneSize; column++)
						_cells[row, column] = new UndiscoveredCell(row, column);
				}
			}

			void PutBombs()
			{
				var generator = new BombGenerator(numberOfBombs);
				do
				{
					_cells.OfType<UndiscoveredCell>()
						.Where(a=> !a.HasBomb)
						.ForEach(a => a.HasBomb = generator.Put());

				} while (generator.BombNumber > 0);

			}

			void ScanNumberOfBombsAroundEmptyCells()
			{
				_cells.OfType<UndiscoveredCell>()
					.Where(a=>a.HasBomb)
					.SelectMany(CellsAround)
					.ForEach(u=> ((UndiscoveredCell)u).NumberOfBombsAround++ );
			}
		}


		public ICell this[ICell cell] => this[cell.Row, cell.Column];
		public ICell this[int row, int flag] => _cells[row, flag];

		public static MineField Create(int zoneSize, int numberOfBombs) => new MineField(zoneSize, numberOfBombs);

		public static MineField Build(int zoneSize, IEnumerable<ICell> cells) => new MineField(zoneSize, cells);

		public IEnumerator<ICell> GetEnumerator()
		{
			foreach (var cell in _cells)
				yield return cell;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void PoseFlag(ICell cell) => PoseFlag(cell.Row, cell.Column);
		public void PoseFlag(int row, int column)
		{
			if(IsOutOfRange(row, column)) return;

			switch (_cells[row, column])
			{
				case UndiscoveredCell c :
					if(FlaggedCellCount >= BombCount) return;
					_cells[row, column] = c.ToFlaggedCell();
					FlaggedCellCount++;
					break;

				case FlaggedCell f:
					_cells[row, column] = f.ToUndiscoveredCell();
					FlaggedCellCount--;
					break;
			}
		}

		public void Reveal(ICell cell) => Reveal(cell.Row, cell.Column);
		public void Reveal(int row, int column)
		{
			if (IsOutOfRange(row, column)) return;

			var cell = _cells[row, column] as UndiscoveredCell;
			if(cell == null) return;

			if (cell.HasBomb)
			{
				_cells[row, column] = new BombCell(row,column);
				return;
			}

			var emptyCell = new EmptyCell(row, column);
			_cells[row, column] = emptyCell;

			CellsAround(emptyCell).ForEach(a=>
			{
				var c = a as UndiscoveredCell;

				if(c != null && !c.HasBomb)
					_cells[a.Row, a.Column] = c.NumberOfBombsAround > 0 
											  ? c.ToNumberCell() as ICell
											  : c.ToEmptyCell();
			});
		}

		private IEnumerable<ICell> CellsAround(ICell emptyCell)
		{
			if (ElementToReturn(-1, -1) != null) yield return ElementToReturn(-1, -1);
			if (ElementToReturn(-1, 0) != null) yield return ElementToReturn(-1, 0);
			if (ElementToReturn(-1, +1) != null) yield return ElementToReturn(-1, +1);
			
			if (ElementToReturn(0, -1) != null) yield return ElementToReturn(0, -1);
			if (ElementToReturn(0, +1) != null) yield return ElementToReturn(0, +1);

			if (ElementToReturn(1, -1) != null) yield return ElementToReturn(1, -1);
			if (ElementToReturn(1, 0) != null) yield return ElementToReturn(1, 0);
			if (ElementToReturn(1, +1) != null) yield return ElementToReturn(1, +1);

			ICell ElementToReturn(int relativeRow, int relativeColumn)
			{
				return !IsOutOfRange(emptyCell.Row + relativeRow, emptyCell.Column + relativeColumn) 
					? _cells[emptyCell.Row + relativeRow, emptyCell.Column + relativeColumn] 
					: null;
			}
		}

		private class BombGenerator
		{
			public int BombNumber { get; private set; }

			public BombGenerator(int bombNumber)
			{
				BombNumber = bombNumber;
			}

			private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

			public bool Put()
			{
				if (BombNumber == 0) return false;
				if (_random.Next(100) <= 95) return false;
				BombNumber--;
				return true;
			}
		}

		private bool IsOutOfRange(int row, int column)
		{
			if (row < 0 || column < 0)
				return true;

			return row >= ZoneSize || column >= ZoneSize;
		}
	}
}