using System;
using ElasticSearch.Client.Transport.IDL;

namespace ElasticSearch.Client.Transport.Connection
{
	internal interface IConnection : IDisposable
	{
		DateTime Created { get; }
		bool IsOpen { get; }

		Server Server { get; }
		Rest.Client Client { get; }

		void Open();
		void Close();
	}
}