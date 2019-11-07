using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Demineur
{
	public interface ICell
	{
		int Row { get; }
		int Column { get; }
	}
}