using System;

namespace ElasticSearch.Client.Transport.Connection
{
	internal class Server
	{
		public const int DefaultPort = 9500;

		public Server(string host = "127.0.0.1", int port = DefaultPort, bool isFramed = false)
		{
			Host = host;
			Port = port;
			IsFramed = isFramed;
		}

		public int Port { get; private set; }

		public string Host { get; private set; }

		public bool IsFramed { get; set; }

		public override string ToString()
		{
			return String.Concat(Host, ":", Port);
		}
	}
}