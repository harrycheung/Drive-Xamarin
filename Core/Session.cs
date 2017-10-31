using System;
using  System.Collections.Generic;

namespace Drive.Core
{
	public class Session
	{
		public Track track;
		public double startTime;
		public double duration = 0;
		public List<Lap> laps = new List<Lap>();

		public Session(Track track, double startTime) {
			this.track     = track;
			this.startTime = startTime;
		}

		public void Tick(double currentTime) {
			duration = currentTime - startTime;
		}
	}
}

