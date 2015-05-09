using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Microsoft.Xna.Framework;

namespace SpackRunner
{
	[Activity (Label = "SpackRunner"
	, MainLauncher = true
	, Icon = "@drawable/icon"
	, AlwaysRetainTaskState = true
	, LaunchMode = LaunchMode.SingleInstance
	, ScreenOrientation = ScreenOrientation.SensorLandscape
	, ConfigurationChanges = ConfigChanges.Orientation |
	ConfigChanges.Keyboard |
	ConfigChanges.KeyboardHidden)]
	public class MainActivity : AndroidGameActivity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			var g = new MainGame ();
			SetContentView ((View)g.Services.GetService (typeof(View)));
			g.Run ();
		}
	}
}