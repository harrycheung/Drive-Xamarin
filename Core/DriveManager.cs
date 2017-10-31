using System;
using System.Net;
using System.IO;
using System.Text;

namespace Drive.Core
{
	public class DriveManager
	{
		public Track[] tracks = null;
		public Track track = null;

		private static DriveManager instance = new DriveManager();
		public static DriveManager Instance() { return instance; }
		private DriveManager() { }

		public void LoadTracks() {
			try {
				WebRequest request = WebRequest.Create("http://drive.cohesivefront.com/tracks.json");
				// Get the response.
				WebResponse response = request.GetResponse();
				// Display the status.
				Console.WriteLine(((HttpWebResponse)response).StatusDescription);
				// Get the stream containing content returned by the server.
				Stream dataStream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.
				string responseFromServer = reader.ReadToEnd();
				tracks = Track.Load(responseFromServer);
				// Display the content.
				Console.WriteLine(responseFromServer);
				// Clean up the streams and the response.
				reader.Close();
				response.Close();
				Console.WriteLine("Loaded " + tracks.Length + " tracks.");
			} catch (IOException e) {
				Console.WriteLine("" + e);
			}
		}
	}
}

