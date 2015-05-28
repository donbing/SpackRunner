using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using BloomPostprocess;

namespace SpackRunner
{
	public class MainGame : Game
	{
		readonly GraphicsDeviceManager graphics;
		ArcBallCamera cam;
		Controls controls;
		BloomComponent bloom;

		public MainGame ()
		{
			Content.RootDirectory = "Content";
			graphics = new GraphicsDeviceManager (this);
			//bloom = new BloomComponent (this);
			//Components.Add (bloom);
			//bloom.Settings = new BloomSettings (null, 0.25f, 4, 2, 1, 1.5f, 1);
		}

		protected override void Initialize ()
		{
			graphics.IsFullScreen = true;
			graphics.ApplyChanges ();

			cam = new ArcBallCamera (Window.ClientBounds.Width / Window.ClientBounds.Height, Vector3.Zero);
			controls = new Controls (cam);

			Components.Add (new GameOfLifeComponent (this, cam, controls));

			Components.Add (new StringOverlay (this) {
				() => GameOfLifeComponent.averagegenerationtime.ToString (),
				() => cam.Position.ToString (),
			});

			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			Services.AddService (new SpriteBatch (GraphicsDevice));
			base.LoadContent ();
		}

		protected override void Update (GameTime gameTime)
		{
			if (TouchPanel.IsGestureAvailable) {
				controls.MeasureControlsState (TouchPanel.ReadGesture ());
			}

			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			
			GraphicsDevice.Clear (ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
			//bloom.BeginDraw ();
			base.Draw (gameTime);
		}
	}
}
