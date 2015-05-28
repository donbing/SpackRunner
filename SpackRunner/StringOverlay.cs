using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections;

namespace SpackRunner
{
	public class StringOverlay : DrawableGameComponent, IEnumerable
	{
		public IEnumerator GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		SpriteFont fontSprite;
		IList<Func<string>> funs = new List<Func<string>> ();

		public void Add (Func<string> func)
		{
			funs.Add (func);
		}

		public StringOverlay (Game game) : base (game)
		{
		}

		protected override void LoadContent ()
		{
			fontSprite = Game.Content.Load<SpriteFont> ("font");
			base.LoadContent ();
		}

		public override void Initialize ()
		{
			base.Initialize ();
		}

		public override void Draw (GameTime gameTime)
		{
			var spriteBatch = Game.Services.GetService<SpriteBatch> ();
			var pos = Vector2.Zero;

			spriteBatch.Begin ();

			foreach (var message in funs) {
				spriteBatch.DrawString (fontSprite, message (), pos, Color.White);	
				pos += new Vector2 (0, 20);
			}

			spriteBatch.End ();

			var d = new DepthStencilState ();
			d.DepthBufferEnable = true;
			GraphicsDevice.DepthStencilState = d;

			base.Draw (gameTime);
		}
	}

}
