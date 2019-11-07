using System;

namespace Demineur
{
	public class FlaggedCell : Cell 
	{
		private readonly UndiscoveredCell _source;

		public bool HasBomb => _source.HasBomb;

		public FlaggedCell(int row, int column, UndiscoveredCell source) : base(row, column)
		{
			_source = source ?? throw new ArgumentNullException(nameof(source));
		}

		public UndiscoveredCell ToUndiscoveredCell() => _source;
	}
}