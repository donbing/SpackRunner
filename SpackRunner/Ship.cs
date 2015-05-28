using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpackRunner
{
	class Ship
	{
		public Model Model;
		public Matrix[] Transforms;

		//Position of the model in world space
		public Vector3 Position = Vector3.Zero;

		//Velocity of the model, applied each frame to the model's position
		public Vector3 Velocity = Vector3.Zero;

		public Matrix RotationMatrix = Matrix.Identity;
		private float rotation;

		public float Rotation {
			get { return rotation; }
			set {
				float newVal = value;
				while (newVal >= MathHelper.TwoPi) {
					newVal -= MathHelper.TwoPi;
				}
				while (newVal < 0) {
					newVal += MathHelper.TwoPi;
				}
				if (rotation != newVal) {
					rotation = newVal;
					RotationMatrix = Matrix.CreateRotationY (rotation);
				}
			}
		}

		public void Update (GamePadState controllerState)
		{
			// Rotate the model using the left thumbstick, and scale it down.
			Rotation -= controllerState.ThumbSticks.Left.X * 0.10f;

			// Finally, add this vector to our velocity.
			Velocity += RotationMatrix.Forward * 1.0f * controllerState.Triggers.Right;
		}

		public static void DrawModel (Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
		{
			foreach (ModelMesh mesh in model.Meshes) {
				foreach (BasicEffect effect in mesh.Effects) {
					effect.World = absoluteBoneTransforms [mesh.ParentBone.Index] * modelTransform;
				}
				mesh.Draw ();
			}
		}
	}

}