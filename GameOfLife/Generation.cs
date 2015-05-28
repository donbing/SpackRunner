using System;
using System.Collections.Generic;
using System.Linq;

namespace SpackRunner
{
	public class Generation
	{
		public readonly HashSet<Cell> startingCells;

		public Generation (IEnumerable<Cell> startingCells)
		{
			this.startingCells = new HashSet<Cell> (startingCells);
		}

		public Generation AddCells (IEnumerable<Cell> organism)
		{
			foreach (var org in organism) {
				startingCells.Add (org);
			}
			return this;
		}

		public Generation Next ()
		{
			var allNeighbours = new HashSet<Cell> (startingCells.SelectMany (x => x.Neighbours ()));

			var deadNeighbours = allNeighbours.Except (startingCells);

			var livingDead = deadNeighbours.AsParallel ().Where (DeadCellShouldSpawn);
			var survivors = startingCells.AsParallel ().Where (LiveCellShouldSurvive);

			var nextGeneration = survivors.Concat (livingDead).ToList ();
			return new Generation (nextGeneration);
		}

		public bool DeadCellShouldSpawn (Cell cell)
		{
			return  GetLiveNeighbours (cell) == 3;
		}

		public bool LiveCellShouldSurvive (Cell cell)
		{
			var livingNeighbourCount = GetLiveNeighbours (cell);

			return livingNeighbourCount == 3 || livingNeighbourCount == 2;
		}

		public int GetLiveNeighbours (Cell cell)
		{
			return cell.Neighbours ().Count (startingCells.Contains);
		}
	}
}