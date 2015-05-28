using System;
using System.Collections.Generic;
using System.Linq;

namespace SpackRunner
{
	public struct Cell : IEquatable<Cell>
	{
		static Cell zeroPoint;
		public readonly int X, Y;

		public static Cell Zero {
			get { return zeroPoint; }
		}

		public Cell (int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public Cell (int value)
		{
			this.X = value;
			this.Y = value;
		}

		public static Cell operator + (Cell value1, Cell value2)
		{
			return new Cell (value1.X + value2.X, value1.Y + value2.Y);
		}

		public static Cell operator - (Cell value1, Cell value2)
		{
			return new Cell (value1.X - value2.X, value1.Y - value2.Y);
		}

		public static Cell operator * (Cell value1, Cell value2)
		{
			return new Cell (value1.X * value2.X, value1.Y * value2.Y);
		}

		public static Cell operator / (Cell source, Cell divisor)
		{
			return new Cell (source.X / divisor.X, source.Y / divisor.Y);
		}

		public static bool operator == (Cell a, Cell b)
		{
			return a.Equals (b);
		}

		public static bool operator != (Cell a, Cell b)
		{
			return !a.Equals (b);
		}

		public override bool Equals (object obj)
		{
			return (obj is Cell) && Equals ((Cell)obj);
		}

		public bool Equals (Cell other)
		{
			return ((X == other.X) && (Y == other.Y));
		}

		public override int GetHashCode ()
		{
			return X ^ Y;
		}

		public override string ToString ()
		{
			return "{X:" + X + " Y:" + Y + "}";
		}
	}
}