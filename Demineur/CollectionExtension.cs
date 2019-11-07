using System;
using System.Collections.Generic;

namespace Demineur
{
	public static class CollectionExtension
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
			{
				action(item);
			}
		}
	}
}