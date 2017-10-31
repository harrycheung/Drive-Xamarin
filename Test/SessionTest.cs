using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Drive.Core;

namespace Drive.Test
{
	[TestFixture()]
	public class SessionTest
	{
		[SetUp]
		public void Before()
		{
			String trackJSON = ""
				+ "{"
				+   "\"track\": {"
				+     "\"id\": \"1000\","
				+     "\"name\": \"Isabella Raceway\","
				+     "\"gates\": ["
				+       "{"
				+       "\"gate_type\": \"SPLIT\","
				+       "\"latitude\": \"37.451775\","
				+       "\"longitude\": \"-122.203657\","
				+       "\"bearing\": \"136\","
				+       "\"split_number\": \"1\""
				+       "},"
				+       "{"
				+       "\"gate_type\": \"SPLIT\","
				+       "\"latitude\": \"37.450127\","
				+       "\"longitude\": \"-122.205499\","
				+       "\"bearing\": \"326\","
				+       "\"split_number\": \"2\""
				+       "},"
				+       "{"
				+       "\"gate_type\": \"START_FINISH\","
				+       "\"latitude\": \"37.452602\","
				+       "\"longitude\": \"-122.207069\","
				+       "\"bearing\": \"32\","
				+       "\"split_number\": \"3\""
				+       "}"
				+     "]"
				+   "}"
				+ "}";
						
			DriveManager.Instance().track = Track.Load(trackJSON)[0];
		}

		[TearDown]
		public void After()
		{
			SessionManager.Instance().End();
		}

		[Test()]
		public void TestSingleLap() 
		{
			SessionManager manager = SessionManager.Instance();
			double startTime = Helper.CurrentTime();
			manager.Start();

			string content = File.ReadAllText("Assets/single_lap_session.csv");
			string[] lines = content.Split('\n');

			foreach (string line in lines) {
				String[] parts = line.Split(',');
				manager.GPS(Double.Parse(parts[0]),
					Double.Parse(parts[1]),
					Double.Parse(parts[2]),
					Double.Parse(parts[3]),
					5.0, 15.0,
					startTime);
				startTime += 1;
			}

			List<Lap> laps = manager.session.laps;
			Assert.AreEqual(3, laps.Count);
			Assert.False(laps[0].valid);
			Assert.True(laps[1].valid);
			Assert.False(laps[2].valid);

			Assert.AreEqual(120.64222, laps[1].duration, 0.00001);

			Assert.AreEqual(manager.bestLap.duration, laps[1].duration);

			Assert.AreEqual(35.85215, laps[1].splits[0], 0.00001);
			Assert.AreEqual(38.94201, laps[1].splits[1], 0.00001);
			Assert.AreEqual(45.84806, laps[1].splits[2], 0.00001);

			Assert.AreEqual(1298.63, laps[1].distance, 0.01);
		}

		[Test()]
		public void TestMultiLap() {
			SessionManager manager = SessionManager.Instance();
			double startTime = Helper.CurrentTime();
			manager.Start();

			string content = File.ReadAllText("Assets/multi_lap_session.csv");
			string[] lines = content.Split('\n');

			foreach (string line in lines) {
				String[] parts = line.Split(',');
				manager.GPS(Double.Parse(parts[0]),
					Double.Parse(parts[1]),
					Double.Parse(parts[2]),
					Double.Parse(parts[3]),
					5.0, 15.0,
					startTime);
				startTime += 1;
			}

			List<Lap> laps = manager.session.laps;
			Assert.AreEqual(6, laps.Count);
			Assert.False(laps[0].valid);
			Assert.True(laps[1].valid);
			Assert.True(laps[2].valid);
			Assert.True(laps[3].valid);
			Assert.True(laps[4].valid);
			Assert.False(laps[5].valid);

			Assert.AreEqual(120.64222, laps[1].duration, 0.00001);
			Assert.AreEqual(100.01685, laps[2].duration, 0.00001);
			Assert.AreEqual( 96.74609, laps[3].duration, 0.00001);
			Assert.AreEqual( 94.61198, laps[4].duration, 0.00001);

			Assert.AreEqual(manager.bestLap.duration, laps[4].duration);

			Assert.AreEqual(1298.63, laps[1].distance, 0.01);
			Assert.AreEqual(1298.69, laps[2].distance, 0.01);
			Assert.AreEqual(1306.85, laps[3].distance, 0.01);
			Assert.AreEqual(1306.55, laps[4].distance, 0.01);
		}
	}
}

