using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpackRunner
{
	public class ArcBallCamera
	{

		public ArcBallCamera (float aspectRation, Vector3 lookAt)
			: this (aspectRation, MathHelper.PiOver4, lookAt, 10f, 5000f)
		{
		}

		public ArcBallCamera (float aspectRatio, float fieldOfView, Vector3 lookAt, float nearPlane, float farPlane)
		{
			this.aspectRatio = aspectRatio;
			this.fieldOfView = fieldOfView;            
			this.lookAt = lookAt;
			this.nearPlane = nearPlane;
			this.farPlane = farPlane;
		}

		/// <summary>
		/// Recreates our view matrix, then signals that the view matrix
		/// is clean.
		/// </summary>
		private void ReCreateViewMatrix ()
		{
			//Calculate the relative position of the camera                        
			position = Vector3.Transform (Vector3.Backward, Matrix.CreateFromYawPitchRoll (yaw, pitch, 0));
			//Convert the relative position to the absolute position
			position *= zoom;
			position += lookAt;

			//Calculate a new viewmatrix
			viewMatrix = Matrix.CreateLookAt (position, lookAt, Vector3.Up);
			viewMatrixDirty = false;
		}

		/// <summary>
		/// Recreates our projection matrix, then signals that the projection
		/// matrix is clean.
		/// </summary>
		private void ReCreateProjectionMatrix ()
		{
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView (MathHelper.PiOver4, aspectRatio, nearPlane, farPlane);
			projectionMatrixDirty = false;
		}

		#region HelperMethods

		/// <summary>
		/// Moves the camera and lookAt at to the right,
		/// as seen from the camera, while keeping the same height
		/// </summary>        
		public void MoveCameraRight (float amount)
		{
			Vector3 right = Vector3.Normalize (LookAt - Position); //calculate forward
			right = Vector3.Cross (right, Vector3.Up); //calculate the real right
			right.Y = 0;
			right.Normalize ();
			LookAt += right * amount;
		}

		/// <summary>
		/// Moves the camera and lookAt forward,
		/// as seen from the camera, while keeping the same height
		/// </summary>        
		public void MoveCameraForward (float amount)
		{
			Vector3 forward = Vector3.Normalize (LookAt - Position);
			forward.Y = 0;
			forward.Normalize ();
			LookAt += forward * amount;
		}

		#endregion

		#region FieldsAndProperties

		//We don't need an update method because the camera only needs updating
		//when we change one of it's parameters.
		//We keep track if one of our matrices is dirty
		//and reacalculate that matrix when it is accesed.
		private bool viewMatrixDirty = true;
		private bool projectionMatrixDirty = true;

		public float MinPitch = -MathHelper.PiOver2 + 0.3f;
		public float MaxPitch = MathHelper.PiOver2 - 0.3f;
		private float pitch;

		public float Pitch {
			get { return pitch; }
			set {
				viewMatrixDirty = true;
				pitch = MathHelper.Clamp (value, MinPitch, MaxPitch);               
			}
		}

		private float yaw;

		public float Yaw {
			get { return yaw; }
			set {
				viewMatrixDirty = true;
				yaw = value;
			}
		}

		private float fieldOfView;

		public float FieldOfView {
			get { return fieldOfView; }
			set {
				projectionMatrixDirty = true;
				fieldOfView = value;
			}
		}

		private float aspectRatio;

		public float AspectRatio {
			get { return aspectRatio; }
			set {
				projectionMatrixDirty = true;
				aspectRatio = value;
			}
		}

		private float nearPlane;

		public float NearPlane {
			get { return nearPlane; }
			set {
				projectionMatrixDirty = true;
				nearPlane = value;
			}
		}

		private float farPlane;

		public float FarPlane {
			get { return farPlane; }
			set {
				projectionMatrixDirty = true;
				farPlane = value;
			}
		}

		public float MinZoom = 1;
		public float MaxZoom = float.MaxValue;
		private float zoom = 1;

		public float Zoom {
			get { return zoom; }
			set {
				viewMatrixDirty = true;
				zoom = MathHelper.Clamp (value, MinZoom, MaxZoom);
			}
		}

		private Vector3 position;

		public Vector3 Position {
			get {
				if (viewMatrixDirty) {
					ReCreateViewMatrix ();
				}
				return position;
			}
		}

		private Vector3 lookAt;

		public Vector3 LookAt {
			get { return lookAt; }
			set {
				viewMatrixDirty = true;
				lookAt = value;
			}
		}

		#endregion

		#region ICamera Members

		public Matrix ViewProjectionMatrix {
			get { return ViewMatrix * ProjectionMatrix; }
		}

		private Matrix viewMatrix;

		public Matrix ViewMatrix {
			get {
				if (viewMatrixDirty) {
					ReCreateViewMatrix ();
				}
				return viewMatrix;
			}
		}

		private Matrix projectionMatrix;

		public Matrix ProjectionMatrix {
			get {
				if (projectionMatrixDirty) {
					ReCreateProjectionMatrix ();
				}
				return projectionMatrix;
			}
		}

		#endregion

		public Ray GetNormalRay (Vector2 touchPoint, Viewport viewport)
		{
			var nearPoint = new Vector3 (touchPoint, 0f);
			var farPoint = new Vector3 (touchPoint, 1f);

			var u1 = viewport.Unproject (nearPoint, ProjectionMatrix, ViewMatrix, Matrix.Identity);
			var u2 = viewport.Unproject (farPoint, ProjectionMatrix, ViewMatrix, Matrix.Identity);

			return new Ray (u1, Vector3.Normalize (u2 - u1));
		}

		public void SetLights (BasicEffect effect)
		{
			effect.EnableDefaultLighting ();

			effect.LightingEnabled = true;

			effect.DirectionalLight0.Enabled = true;
			effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3 ();
			effect.DirectionalLight0.Direction = Vector3.Normalize (new Vector3 (-1, -1.5f, 0));
			effect.DirectionalLight0.SpecularColor = Color.White.ToVector3 ();

			effect.DirectionalLight1.Enabled = true;
			effect.DirectionalLight1.DiffuseColor = Color.Green.ToVector3 ();
			effect.DirectionalLight1.Direction = Vector3.Normalize (new Vector3 (1, -1.5f, -1));
			//effect.DirectionalLight2.SpecularColor = Color.White.ToVector3 ();

			effect.DirectionalLight2.Enabled = true;
			effect.DirectionalLight2.DiffuseColor = Color.BlueViolet.ToVector3 ();
			effect.DirectionalLight2.Direction = Vector3.Normalize (new Vector3 (0, -1.5f, -1));
			//effect.DirectionalLight2.SpecularColor = Color.White.ToVector3 ();

			effect.SpecularColor = Color.White.ToVector3 ();

			effect.AmbientLightColor = new Vector3 (.1f, .1f, .1f);
			//effect.EmissiveColor = new Vector3 (1, 0, 0);
		}
	}
}
