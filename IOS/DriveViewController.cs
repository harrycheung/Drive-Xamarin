using System;
using UIKit;
using Drive.Core;
using System.Collections.Generic;

namespace Drive.IOS
{
	partial class DriveViewController : UIViewController
	{
		public DriveViewController(IntPtr handle) : base(handle)
		{
			DriveManager.Instance().LoadTracks();
		}

		partial void ClickDrive(UIButton sender)
		{
			foreach(Track track in DriveManager.Instance().tracks) {
				if (track.name == "Isabella Raceway") {
					DriveManager.Instance().track = track;
				}
			}

			string contents = System.IO.File.ReadAllText("Assets/multi_lap_session.csv");
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
			sender.SetTitle("" + Math.Round(t.TotalSeconds - start, 3), UIControlState.Normal);
		}
	}
}
