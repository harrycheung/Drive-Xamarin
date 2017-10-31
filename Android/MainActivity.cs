using System;
using System.Collections.Generic;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Drive.Core;

namespace Android
{
	[Activity(Label = "Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Android.Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			DriveManager.Instance().LoadTracks();
			
			button.Click += delegate {
				foreach(Track track in DriveManager.Instance().tracks) {
					if (track.name == "Isabella Raceway") {
						DriveManager.Instance().track = track;
					}
				}

				string contents;
				using (var sr = new StreamReader (Assets.Open ("multi_lap_session.csv")))
				{
					contents = sr.ReadToEnd();
				}
				string[] lines = contents.Split('\n');
				List<Point> points = new List<Point>();
				foreach(string line in lines) {
					string[] parts = line.Split(',');
					points.Add(new Point(
						latitude: Double.Parse(parts[0]),
						longitude: Double.Parse(parts[1]),
						speed: Double.Parse(parts[2]),
						bearing: Double.Parse(parts[3]),
						horizontalAccuracy: 5.0,
						verticalAccuracy: 15.0,
						timestamp: 0));
				}
				Console.WriteLine("loaded points");
				TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
				int start = (int)t.TotalSeconds;
				for (int i = 0; i < 10000; i++) {
					t = DateTime.UtcNow - new DateTime(1970, 1, 1);
					int startTime = (int)t.TotalSeconds;
					SessionManager.Instance().Start();
					foreach(Point point in points) {
						SessionManager.Instance().GPS(point.LatitudeDegrees(), longitude: point.LongitudeDegrees(), speed: point.speed, bearing: point.bearing, horizontalAccuracy: point.hAccuracy, verticalAccuracy: point.vAccuracy, timestamp: startTime);
						startTime += 1;
					}
					SessionManager.Instance().End();
				}
				t = DateTime.UtcNow - new DateTime(1970, 1, 1);
				button.Text = "" + Math.Round(t.TotalSeconds - start, 3);
			};
		}
	}
}


