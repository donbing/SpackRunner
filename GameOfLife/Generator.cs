using System;
using System.Collections.Generic;
using System.Linq;

namespace SpackRunner
{
	public class Generator
	{
		string glider = @"
.O. 
..O 
OOO";
		const string glidergun = @"
........................O........... 
......................O.O........... 
............OO......OO............OO 
...........O...O....OO............OO 
OO........O.....O...OO.............. 
OO........O...O.OO....O.O........... 
..........O.....O.......O........... 
...........O...O.................... 
............OO......................";

		public static Generation Random ()
		{
			var random = new Random ();

			var points = Enumerable.Range (0, 1000)
				.Select (x => new { x = (random.NextDouble () - 0.5) * 50, y = (random.NextDouble () - 0.5) * 50})
				.Select (x => new Cell ((int)x.x, (int)x.y));

			return new Generation (points);
		}

		public static IEnumerable<Cell> OrganismAt (int x, int y)
		{
			return OrganismFromString (glidergun).Select (c => c + new Cell (x, y));
		}

		public static IEnumerable<Cell> OrganismFromString (string picture)
		{
			return picture.Split (new[]{ Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
				.SelectMany ((rowChars, rowIndex) => 
					rowChars.ToCharArray ()
						.Select ((character, colIndex) => new{character, colIndex})
						.Where (c => c.character == 'O')
						.Select (c => new Cell (rowIndex, c.colIndex))
			).ToList ();
		}

		public static Generation FourTShapes ()
		{
			var shapes = OrganismFromString (glidergun);
			var mess = shapes.Concat (FourTShapesOld ());
			return new Generation (mess);
		}

		public static  IEnumerable<Cell> FourTShapesOld ()
		{
			var shapes = new[] { 
				GliderAt (new Cell (-4, 4)).Invert (),
				GliderAt (new Cell (-7, 9)).Invert (),

				GliderAt (new Cell (-10, 3)).Invert (),
				GliderAt (new Cell (-7, 9)).Invert (),

				GliderAt (new Cell (0, 0)).Invert (),
				GliderAt (new Cell (4, 5)),
				CreateTShapeAt (new Cell (9, 8)),
				GliderAt (new Cell (0, 10)),
				GliderAt (new Cell (10, 0)),
				CreateTShapeAt (new Cell (0, 25)),
				GliderAt (new Cell (4, -13)).Invert (),
				GliderAt (new Cell (-8, -7)),
				CreateTShapeAt (new Cell (0, 25)),
				GliderAt (new Cell (20, 27)),
				GliderAt (new Cell (14, 15)).Invert (),
				CreateTShapeAt (new Cell (19, 18)),
				GliderAt (new Cell (-16, 10)),
				GliderAt (new Cell (-10, 1)),
				CreateTShapeAt (new Cell (6, 10)),
				GliderAt (new Cell (-2, -30)),
				GliderAt (new Cell (-20, 7)),
				CreateTShapeAt (new Cell (-20, -20)),
			};
			return shapes.SelectMany (x => x.AsEnumerable ());
		}

		private static IEnumerable<Cell> CreateTShapeAt (Cell pos)
		{
			return new [] {
				new Cell (pos.X + 0, pos.Y - 1),
				new Cell (pos.X + 0, pos.Y - 0),
				new Cell (pos.X + 0, pos.Y + 1),
				new Cell (pos.X + 1, pos.Y - 0),
			};
		}

		public static IEnumerable<Cell> GliderAt (Cell pos)
		{
			return new [] {
				new Cell (pos.X + 0, pos.Y - 0),
				new Cell (pos.X + 1, pos.Y + 1),
				new Cell (pos.X + 1, pos.Y + 2),
				new Cell (pos.X + 0, pos.Y + 2),
				new Cell (pos.X - 1, pos.Y + 2),
			};
		}
	}
}