using System;

namespace Drive.Core
{
	public static class Helper
	{
		public static double CurrentTime()
		{
			TimeSpan span = DateTime.UtcNow.Subtract(new DateTime(1970,1,1,0,0,0));
			return span.TotalSeconds;
		}
	}
}

