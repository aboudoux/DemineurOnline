namespace Demineur
{
	public abstract class Cell : ICell
	{
		protected Cell(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public int Row { get; }
		public int Column { get; }
	}
}