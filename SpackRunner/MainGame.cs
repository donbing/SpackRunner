using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input.Touch;

namespace SpackRunner
{
	public class MainGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private BasicEffect effect;

		public MainGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.IsFullScreen = true;
			TouchPanel.EnabledGestures = GestureType.HorizontalDrag | GestureType.VerticalDrag | GestureType.DoubleTap | GestureType.Pinch;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			effect = new BasicEffect (graphics.GraphicsDevice);
			//effect.AmbientLightColor = Color.White.ToVector3 ();

			effect.DirectionalLight0.Enabled = true;
			effect.DirectionalLight0.DiffuseColor = Color.Red.ToVector3 ();
			effect.DirectionalLight0.Direction = Vector3.Normalize (new Vector3 (-1, -1.5f, 0));
			effect.DirectionalLight0.SpecularColor = Color.White.ToVector3 ();

			effect.DirectionalLight1.Enabled = true;
			effect.DirectionalLight1.DiffuseColor = Color.Green.ToVector3 ();
			effect.DirectionalLight1.Direction = Vector3.Normalize (new Vector3 (1, -1.5f, -1));
			effect.DirectionalLight2.SpecularColor = Color.White.ToVector3 ();

			effect.DirectionalLight2.Enabled = true;
			effect.DirectionalLight2.DiffuseColor = Color.Blue.ToVector3 ();
			effect.DirectionalLight2.Direction = Vector3.Normalize (new Vector3 (0, -1.5f, -1));
			effect.DirectionalLight2.SpecularColor = Color.White.ToVector3 ();

			effect.SpecularColor = Color.White.ToVector3 ();

			effect.LightingEnabled = true;
			effect.AmbientLightColor = new Vector3 (.1f, .1f, .1f);

			effect.Projection = Matrix.CreatePerspectiveFieldOfView (
				(float)Math.PI / 4.0f, 
				(float)this.Window.ClientBounds.Width /
				(float)this.Window.ClientBounds.Height, 
				1f, 10000f);
			
			effect.View = Matrix.CreateTranslation (0f, 0f, -1000f);

			base.Initialize ();
		}

		IList<BasicShape> cubes = new List<BasicShape> ();

		Generation currentGeneration;

		Model model;

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			model = Content.Load<Model> ("Models\\Animated Blob PC");

		}

		float angle = 0;
		TimeSpan lastGenerationTime;
		Vector3 cubeSize = new Vector3 (0.4f, 0.4f, 0.4f);

		float modelYRotation = 0.5f;
		float horizontalDragGestureValue;

		protected override void Update (GameTime gameTime)
		{
			if (TouchPanel.IsGestureAvailable) {
				var touchReading = TouchPanel.ReadGesture ();
				switch (touchReading.GestureType) {
				case GestureType.HorizontalDrag:
					modelXRotation += touchReading.Delta.X / 100;
					break;
				case GestureType.VerticalDrag:
					modelYRotation += touchReading.Delta.Y / 100;
					break;
				case GestureType.DoubleTap:
					SetGeneration (Generation.FourTShapes, gameTime);
					break;
				case GestureType.Pinch:
					Vector2 a = touchReading.Delta;
					Vector2 b = touchReading.Position2;
					float dist = Vector2.Distance (a, b);
					effect.View = Matrix.CreateTranslation (0f, 0f, -dist / 10);
					break;
				}
			}

			if (currentGeneration != null) {
				if (gameTime.TotalGameTime - lastGenerationTime > TimeSpan.FromSeconds (0.5)) {
					//SetGeneration (currentGeneration.Next (), gameTime);
				}
			}

			angle = angle + horizontalDragGestureValue / 10000;
			if (angle > 2 * Math.PI)
				angle = 0;
			
			Matrix R = Matrix.CreateRotationY (angle) * Matrix.CreateRotationX (modelYRotation);//.8f
			Matrix T = Matrix.CreateTranslation (0f, 0f, 2f); // x,y,z transaltion for the camera world?
			effect.World = R * T;

			base.Update (gameTime);
		}

		void SetGeneration (Generation generation, GameTime gameTime)
		{
			currentGeneration = generation;
			cubes = currentGeneration.startingCells.Select (cell => new BasicShape (cubeSize, new Vector3 (cell.X, 0, cell.Y))).ToList ();
			lastGenerationTime = gameTime.TotalGameTime;
		}

		Vector3 modelPosition = Vector3.Zero;
		float modelXRotation = 0.0f;

		// Set the position of the camera in world space, for our view matrix.
		Vector3 cameraPosition = new Vector3 (0.0f, 0.0f, 25.0f);

		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			var aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
			Matrix[] transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo (transforms);

			foreach (ModelMesh mesh in model.Meshes) {
				// This is where the mesh orientation is set, as well 
				// as our camera and projection.
				foreach (BasicEffect effect2 in mesh.Effects) {
					effect2.EnableDefaultLighting ();
					effect2.World = transforms [mesh.ParentBone.Index] * Matrix.CreateRotationY (modelXRotation) * Matrix.CreateRotationX (modelYRotation);
					effect2.View = Matrix.CreateLookAt (cameraPosition, Vector3.Zero, Vector3.Up);
					effect2.Projection = Matrix.CreatePerspectiveFieldOfView (MathHelper.ToRadians (45.0f), aspectRatio, 1.0f, 10000.0f);
				}
				// Draw the mesh, using the effects set above.
				mesh.Draw ();
			}
			base.Draw (gameTime);
		}
	}
}
