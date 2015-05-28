using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace SpackRunner
{
	public class GameOfLifeComponent : DrawableGameComponent
	{
		public static TimeSpan averagegenerationtime;

		Model model;
		Generation currentGeneration;
		CancellationTokenSource cancellationToken;

		ArcBallCamera cam;

		Ray ray;

		BoardPlane boardPlane;
		Controls con;

		public GameOfLifeComponent (MainGame mainGame, ArcBallCamera cam, Controls con) : base (mainGame)
		{
			this.con = con;
			this.cam = cam;
			currentGeneration = Generator.Random ();
			boardPlane = new BoardPlane ();
		}

		protected override void LoadContent ()
		{
			model = Game.Content.Load<Model> ("Models\\Box + Point Cache Group");
			base.LoadContent ();
		}

		public override void Initialize ()
		{
			con.OnDoubleTap = () => {
				cancellationToken.Cancel ();
				Generate (Generator.Random ());
			};

			con.OnTap = (point) => {
				ray = cam.GetNormalRay (point, GraphicsDevice.Viewport);
				var distanceToPlane = ray.Intersects (boardPlane.boundingBox);

				if (distanceToPlane.HasValue) {
					var planePoint = ray.Position + (distanceToPlane.Value * ray.Direction);
					var organism = Generator.OrganismAt ((int)(planePoint.X / 6f), (int)(planePoint.Z / 6f));
					cancellationToken.Cancel ();
					Generate (currentGeneration.AddCells (organism));
				}
			};

			Generate (currentGeneration);
			base.Initialize ();
		}

		void Generate (Generation generation)
		{
			cancellationToken = new CancellationTokenSource ();

			Task.Delay (TimeSpan.FromMilliseconds (200), cancellationToken.Token)
				.ContinueWith (x => currentGeneration = Time (generation), cancellationToken.Token)
				.ContinueWith (c => Generate (currentGeneration), cancellationToken.Token);
		}

		static Generation Time (Generation currentGeneration)
		{
			using (var timer = ConsoleTimeLogger.Log ("generation")) {
				var generation = currentGeneration.Next ();
				averagegenerationtime = timer.Timed;
				return generation;
			}
		}

		public override void Draw (GameTime gameTime)
		{
			foreach (var cell in currentGeneration.startingCells) {
				DrawCellModel (model, Matrix.CreateTranslation (cell.X * 6, 0, cell.Y * 6));
			}
			var rayPoints = new [] {
				new VertexPositionColor (ray.Position, Color.Black),
				new VertexPositionColor (ray.Position + (ray.Intersects (boardPlane.boundingBox).GetValueOrDefault () * ray.Direction), Color.HotPink)
			};
			boardPlane.Draw (Game.GraphicsDevice, cam, rayPoints);
			base.Draw (gameTime);
		}

		private void DrawCellModel (Model m, Matrix world)
		{
			foreach (var mesh in m.Meshes) {
				foreach (BasicEffect effect in mesh.Effects) {
					effect.EnableDefaultLighting ();
					effect.View = cam.ViewMatrix;
					effect.Projection = cam.ProjectionMatrix;
					effect.World = world;
					cam.SetLights (effect);
				}
				mesh.Draw ();
			}
		}
	}


}
