using System;
using ElasticSearch.Thrift;

namespace ElasticSearch.Client
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