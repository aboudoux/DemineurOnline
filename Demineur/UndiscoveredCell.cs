namespace Demineur
{
	public class UndiscoveredCell : Cell 
	{
		public bool HasBomb { get;  set; }

		public int NumberOfBombsAround { get; set; }

		public UndiscoveredCell(int row, int column) : base(row, column)
		{
		}

		public EmptyCell ToEmptyCell() => new EmptyCell(Row, Column);
		public NumberCell ToNumberCell() => new NumberCell(Row, Column, NumberOfBombsAround);
		public FlaggedCell ToFlaggedCell() => new FlaggedCell(Row, Column, this);
	}
}