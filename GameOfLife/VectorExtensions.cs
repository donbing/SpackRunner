using System;
using System.Collections.Generic;
using System.Linq;

namespace SpackRunner
{
	public static class VectorExtensions
	{
		public static readonly List<Cell> blockOfEight = Enumerable.Range (-1, 3)
			.SelectMany (row => Enumerable.Range (-1, 3).Select (col => new Cell (row, col)))
			.Except (new[]{ Cell.Zero })
			.ToList ();

		public static IEnumerable<Cell> Neighbours (this Cell cell)
		{
			return blockOfEight.Select (col => col + cell);
		}

		public static IEnumerable<Cell> Invert (this IEnumerable<Cell> positions)
		{
			foreach (var vector in positions) {
				yield return vector * new Cell (-1, -1);
			}
		}
	}
}