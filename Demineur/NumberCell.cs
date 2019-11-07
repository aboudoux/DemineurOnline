namespace Demineur
{
	public class NumberCell : Cell 
	{
		public int Number { get; }

		public NumberCell(int row, int column, int number) : base(row, column)
		{
			Number = number;
		}
	}
}