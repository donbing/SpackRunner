using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SpackRunner
{
	public class Generation
	{
		public static Generation FourTShapes { 
			get { 
				var shapes = new[] { 
					GliderAt (new Vector2 (0, 0)).Invert (),
					GliderAt (new Vector2 (4, 5)),
					CreateTShapeAt (new Vector2 (9, 8)),
					GliderAt (new Vector2 (0, 10)),
					GliderAt (new Vector2 (10, 0)),
					CreateTShapeAt (new Vector2 (0, 25)),
					GliderAt (new Vector2 (4, -13)).Invert (),
					GliderAt (new Vector2 (-8, -7)),
					CreateTShapeAt (new Vector2 (0, 25)),
					GliderAt (new Vector2 (20, 27)),
					GliderAt (new Vector2 (14, 15)).Invert (),
					CreateTShapeAt (new Vector2 (19, 18)),
					GliderAt (new Vector2 (-16, 10)),
					GliderAt (new Vector2 (-10, 1)),
					CreateTShapeAt (new Vector2 (6, 10)),
					GliderAt (new Vector2 (-2, -36)),
					GliderAt (new Vector2 (-27, 7)),
					CreateTShapeAt (new Vector2 (-20, -20)),
				};
				return new Generation (new HashSet<Vector2> (shapes.SelectMany (x => x.AsEnumerable ())));
			}
		}

		public static IEnumerable<Vector2> CreateTShapeAt (Vector2 pos)
		{
			return new [] {
				new Vector2 (pos.X + 0, pos.Y - 1),
				new Vector2 (pos.X + 0, pos.Y - 0),
				new Vector2 (pos.X + 0, pos.Y + 1),
				new Vector2 (pos.X + 1, pos.Y - 0),
			};
		}

		public static IEnumerable<Vector2> GliderAt (Vector2 pos)
		{
			return new [] {
				new Vector2 (pos.X + 0, pos.Y - 0),
				new Vector2 (pos.X + 1, pos.Y + 1),
				new Vector2 (pos.X + 1, pos.Y + 2),
				new Vector2 (pos.X + 0, pos.Y + 2),
				new Vector2 (pos.X - 1, pos.Y + 2),
			};
		}

		public HashSet<Vector2> startingCells;

		public Generation (HashSet<Vector2> startingCells)
		{
			this.startingCells = startingCells;
		}

		List<Vector2> nextGeneration;

		public Generation Next ()
		{
			var d = DateTime.Now;
			Console.Out.WriteLine (d);
			var neighbourHood = new HashSet<Vector2> (startingCells.SelectMany (x => x.SelfAndNeighbours ()));
			Console.Out.WriteLine (DateTime.Now - d);
			nextGeneration = neighbourHood.Where (CellShouldLive).ToList ();
			Console.Out.WriteLine (DateTime.Now - d);
			var hashSet = new HashSet<Vector2> (nextGeneration);
			Console.Out.WriteLine (DateTime.Now - d);
			return new Generation (hashSet);
		}

		public bool CellShouldLive (Vector2 x)
		{
			var livingNeighbours = x.Neighbours ().Intersect (startingCells);
			var livingNeighbourCount = livingNeighbours.Count ();

			return livingNeighbourCount == 3 || (livingNeighbourCount == 2 && startingCells.Contains (x));
		}
	}

	public static class Est
	{
		public static readonly List<Vector2> blockOfNine = Enumerable.Range (-1, 3)
			.SelectMany (row => Enumerable.Range (-1, 3).Select (col => new Vector2 (row, col)))
			.ToList ();

		public static IEnumerable<Vector2> SelfAndNeighbours (this Vector2 vec)
		{
			return blockOfNine.Select (col => col + vec);
		}

		public static IEnumerable<Vector2> Neighbours (this Vector2 vec)
		{
			return SelfAndNeighbours (vec).Except (new[]{ vec });
		}

		public static IEnumerable<Vector2> Invert (this IEnumerable<Vector2> positions)
		{
			foreach (var vector in positions) {
				yield return vector * new Vector2 (-1, -1);
			}
		}
	}
}