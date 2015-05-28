using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input.Touch;

namespace SpackRunner
{
	public class Controls
	{
		public Action<Vector2> OnTap { get; set; }

		readonly ArcBallCamera cam;

		public Controls (ArcBallCamera cam)
		{
			TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DoubleTap | GestureType.Pinch | GestureType.Tap;
			this.cam = cam;
			cam.Zoom = 200;
		}

		public Action OnDoubleTap { get; set; }

		public void MeasureControlsState (GestureSample touchReading)
		{
			switch (touchReading.GestureType) {
			case GestureType.FreeDrag:
				cam.Yaw -= touchReading.Delta.X / 100;
				cam.Pitch -= touchReading.Delta.Y / 100;
				break;
			case GestureType.DoubleTap:
				OnDoubleTap ();
				break;
			case GestureType.Tap:
				OnTap (touchReading.Position);
				break;
			case GestureType.Pinch:
				Vector2 a = touchReading.Position - touchReading.Delta;
				Vector2 b = touchReading.Position2 - touchReading.Delta2;
				float olddist = Vector2.Distance (a, b);
				float newdist = Vector2.Distance (touchReading.Position, touchReading.Position2);

				cam.Zoom += olddist - newdist;
				break;
			}
		}
	}
}
